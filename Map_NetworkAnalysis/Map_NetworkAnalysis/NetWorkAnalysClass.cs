using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.NetworkAnalysis;
using ESRI.ArcGIS.NetworkAnalyst;
using ESRI.ArcGIS.SpatialAnalystTools;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Carto;

namespace Map_NetworkAnalysis
{
    class NetWorkAnalysClass
    {
        //该函数是根据该传入的程序路径全path，截取存放数据的文件夹的路径，并返回
        public static string getPath(string path)
        {
            int t;
            for (t = 0; t < path.Length; t++)
            {
                //因为我将数据文件夹data存放在与bin文件夹同级别的地方。所以这里与bin做判断
                if (path.Substring(t, 3) == "bin") { break; }
             }
            string name = path.Substring(0, t - 1);
            return name;
        }

        //打开工作空间
        public static IWorkspace OpenWorkspace(string strGDBName)
        {
            IWorkspaceFactory workspaceFactory;
            workspaceFactory = new FileGDBWorkspaceFactoryClass();
            return workspaceFactory.OpenFromFile(strGDBName,0);
        }

        //加载参与分析的点要素
        public static void LoadNANetworkLocations(string strNAClassName,
            IFeatureClass inputFC, INAContext m_NAContext, double snapTolerance)
        {
            //ITable b1 = inputFC as ITable;//将传入的要素类转化为一个Table对象，这个要素类如站点类、障碍点类等。
            //int i1 = b1.RowCount(null);//获取行数
            INAClass naClass;
            INamedSet classes;
            classes = m_NAContext.NAClasses;//上下文对象的有关网络分析类的对象集合NamedSet属性
            naClass = classes.get_ItemByName(strNAClassName) as INAClass;//通过传入的名称参数，往classes中获取对应的NAClass
            naClass.DeleteAllRows();//删除naClass中的所有要素
            INAClassLoader classLoader = new NAClassLoader();//新建一个NAClassLoader对象
            classLoader.Locator = m_NAContext.Locator;//将上下文对象的locator属性赋给classLoader，locator用于确定网络上要加载的要素的位置。
            if (snapTolerance > 0) classLoader.Locator.SnapTolerance = snapTolerance;//设置容差
            classLoader.NAClass = naClass;//给classLoader对象的NAClass属性赋值

            //设置字段映射
            INAClassFieldMap fieldMap = null;
            fieldMap = new NAClassFieldMap();
            fieldMap.set_MappedField("FID", "FID");//set_MappedField方法用于建立源数据到NAClass类的映射
            classLoader.FieldMap = fieldMap;//给classLoader对象的FieldMap属性赋值
            int rowsIn = 0;
            int rowLocated = 0;
            IFeatureCursor featureCursor = inputFC.Search(null, true);
            classLoader.Load((ICursor)featureCursor, null, ref rowsIn, ref rowLocated);//ref类型关键字表示对于该参数，在函数过程中可以读也可以写，相当于引用传递。引用传递参数允许函数成员更改参数的值，并保持该更改。
            ((INAContextEdit)m_NAContext).ContextChanged();

        }
        //创建网络分析上下文
        public static INAContext CreatePathSolverContext(INetworkDataset networkDataset)
        {
            IDENetworkDataset deNDS = GetPathDENetworkDataset(networkDataset);
            INASolver naSolver;
            naSolver = new NARouteSolver();
            INAContextEdit contextEdit = naSolver.CreateContext(deNDS, naSolver.DisplayName) as INAContextEdit;
            contextEdit.Bind(networkDataset, new GPMessagesClass());//将网络数据集与网络上下文对象绑定在一起
            //GPMessagesClass对象用于记录错误信息。
            return contextEdit as INAContext;
        }
        public static IDENetworkDataset GetPathDENetworkDataset(INetworkDataset networkDataset)
        {
            //The IDatasetComponent interface is used to access the data element and parent dataset of this dataset.
            IDatasetComponent dsComponent;
            dsComponent = networkDataset as IDatasetComponent;
            //The data element corresponding to the dataset component.DataElement attribute returns a parameter type IDEDataset.
            return dsComponent.DataElement as IDENetworkDataset;

        }
        //根据存放网络数据集的工作空间和网络数据集名称以及要素数据集名称打开网络数据集
        public static INetworkDataset OpenPathNetworkDataset(IWorkspace networkDatasetWorkspace,
            string networkDatasetName, string featureDatasetName)
        {
            if (networkDatasetWorkspace == null || networkDatasetName == ""
                || featureDatasetName == null)
            {
                return null;
            }
            IDatasetContainer3 datasetContainer3 = null;//数据集容器。数据集的集合
            IFeatureWorkspace featureWorkspace = networkDatasetWorkspace as IFeatureWorkspace;
            IFeatureDataset featureDataset;
            featureDataset = featureWorkspace.OpenFeatureDataset(featureDatasetName);//根据名字打开要素数据集
            IFeatureDatasetExtensionContainer featureDataExtensionContainer =
                featureDataset as IFeatureDatasetExtensionContainer;
            IFeatureDatasetExtension featureDatasetExtension = featureDataExtensionContainer.FindExtension(
                esriDatasetType.esriDTNetworkDataset);
            datasetContainer3 = featureDatasetExtension as IDatasetContainer3;
            if (datasetContainer3 == null)
                return null;
            IDataset dataSet = datasetContainer3.get_DatasetByName(esriDatasetType.esriDTNetworkDataset,
                networkDatasetName);
            return dataSet as INetworkDataset;
        }
        //根据图层名获取图层
        public static ILayer GetLayerByName(IMap map ,string layerName)
        {
            if (map == null || layerName == "") return null;
            ILayer layer = null;
            ICompositeLayer compositeLayer = null;
            for (int i = 0; i < map.LayerCount; i++)
            { 
                layer = map.get_Layer(i);
                if (layer is ICompositeLayer || layer is GroupLayer)
                {
                    compositeLayer = layer as ICompositeLayer;
                    for (int j = 0; j < compositeLayer.Count; j++)
                    {
                        if (compositeLayer.get_Layer(j).Name == layerName)
                            return compositeLayer.get_Layer(j);
                    }
                }
                else 
                {
                    if (layer.Name == layerName)
                        return layer;
                }
            }
            return null;
        }
    }
}

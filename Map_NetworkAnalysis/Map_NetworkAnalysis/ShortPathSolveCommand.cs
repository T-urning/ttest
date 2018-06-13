using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.NetworkAnalyst;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using System.Windows.Forms;
using ESRI.ArcGIS.esriSystem;

namespace Map_NetworkAnalysis
{
    /// <summary>
    /// Command that works in ArcMap/Map/PageLayout
    /// </summary>
    [Guid("431b845b-b502-422f-82e5-84f8dd7da09f")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("Map_NetworkAnalysis.ShortPathSolveCommand")]
    public sealed class ShortPathSolveCommand : BaseCommand
    {
        #region COM Registration Function(s)
        [ComRegisterFunction()]
        [ComVisible(false)]
        static void RegisterFunction(Type registerType)
        {
            // Required for ArcGIS Component Category Registrar support
            ArcGISCategoryRegistration(registerType);

            //
            // TODO: Add any COM registration code here
            //
        }

        [ComUnregisterFunction()]
        [ComVisible(false)]
        static void UnregisterFunction(Type registerType)
        {
            // Required for ArcGIS Component Category Registrar support
            ArcGISCategoryUnregistration(registerType);

            //
            // TODO: Add any COM unregistration code here
            //
        }

        #region ArcGIS Component Category Registrar generated code
        /// <summary>
        /// Required method for ArcGIS Component Category registration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryRegistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            MxCommands.Register(regKey);
            ControlsCommands.Register(regKey);
        }
        /// <summary>
        /// Required method for ArcGIS Component Category unregistration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryUnregistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            MxCommands.Unregister(regKey);
            ControlsCommands.Unregister(regKey);
        }

        #endregion
        #endregion

        private IHookHelper m_hookHelper = null;

        public static INAContext m_NAContext;   //定义网络分析上下文
        private INetworkDataset networkDataset;//定义网络数据集
        private IFeatureClass inputFClass;//定义站点要素类
        private IFeatureClass barriesFClass;//定义障碍点要素类
        string path = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;//获取此程序所在路径
        public ShortPathSolveCommand()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = "NetworkAnalyst"; //localizable text
            base.m_caption = "路径解决";  //localizable text 
            base.m_message = "得到最短路径";  //localizable text
            base.m_toolTip = "路径解决";  //localizable text
            base.m_name = "ShortPathSolver";   //unique id, non-localizable (e.g. "MyCategory_MyCommand")

            try
            {
                //
                // TODO: change bitmap name if necessary
                //
                string bitmapResourceName = GetType().Name + ".bmp";
                base.m_bitmap = new Bitmap(GetType(), bitmapResourceName);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message, "Invalid Bitmap");
            }
        }

        #region Overridden Class Methods

        /// <summary>
        /// Occurs when this command is created
        /// </summary>
        /// <param name="hook">Instance of the application</param>
        /// 
        public override void OnCreate(object hook)
        {
            if (hook == null)
                return;

            try
            {
                m_hookHelper = new HookHelperClass();
                m_hookHelper.Hook = hook;
                if (m_hookHelper.ActiveView == null)
                    m_hookHelper = null;
            }
            catch
            {
                m_hookHelper = null;
            }

            if (m_hookHelper == null)
                base.m_enabled = false;
            else
                base.m_enabled = true;

            // TODO:  Add other initialization code
        }

        /// <summary>
        /// Occurs when this command is clicked
        /// </summary>
        public override void OnClick()
        {
            string name = NetWorkAnalysClass.getPath(path) + "\\data\\HuanbaoGeodatabase.gdb";//得到数据库文件路径
            IFeatureWorkspace pFWorkspace = NetWorkAnalysClass.OpenWorkspace(name) as IFeatureWorkspace;//打开要素工作空间
            //"RouteNetwork","BaseData"参数不可更改
            networkDataset = NetWorkAnalysClass.OpenPathNetworkDataset(pFWorkspace as IWorkspace,
                "RouteNetwork","BaseData");//打开网络数据集
            m_NAContext = NetWorkAnalysClass.CreatePathSolverContext(networkDataset);//通过网络数据集创建网络分析上下文


            inputFClass = pFWorkspace.OpenFeatureClass("Stops");  //根据名字打开站点要素类
            barriesFClass = pFWorkspace.OpenFeatureClass("Barries");  //根据名字打开障碍点要素类
            if (IFLayerExist("NetworkDataset") == false)//若不存在NetworkDataset图层就执行
            {
                //创建名为“NetworkDataset”的图层
                ILayer layer;
                INetworkLayer networkLayer;
                networkLayer = new NetworkLayerClass();
                networkLayer.NetworkDataset = networkDataset;
                layer = networkLayer as ILayer;
                layer.Name = "NetworkDataset";
                m_hookHelper.ActiveView.FocusMap.AddLayer(layer);
                layer.Visible = false;
            }
            //检查是否存在名为“Route”的图层。若存在则删除
            if (IFLayerExist(m_NAContext.Solver.DisplayName) == true)
            {
                for (int i = 0; i < m_hookHelper.FocusMap.LayerCount; i++)
                {
                    if (m_hookHelper.FocusMap.get_Layer(i).Name == m_NAContext.Solver.DisplayName)
                    {
                        m_hookHelper.FocusMap.DeleteLayer(m_hookHelper.FocusMap.get_Layer(i));
                    }
                }
            }
            INALayer naLayer = m_NAContext.Solver.CreateLayer(m_NAContext);
            ILayer pLayer = naLayer as ILayer;
            pLayer.Name = m_NAContext.Solver.DisplayName;
            m_hookHelper.ActiveView.FocusMap.AddLayer(pLayer);//往当前地图中添加该图层
            

            int featureCount = inputFClass.FeatureCount(null);//得到站点要素类中要素的个数
            if (featureCount < 2)//若少于两个则不能进行分析
            {
                MessageBox.Show("只有一个站点，不能进行路径分析！");
                return;
            }
            IGPMessages gpMessages = new GPMessagesClass();//定义一个地理处理结果信息返回对象
            //加载站点要素，并设置容差
            NetWorkAnalysClass.LoadNANetworkLocations("Stops", inputFClass, m_NAContext, 80);
            //加载障碍点要素，并设置容差
            NetWorkAnalysClass.LoadNANetworkLocations("Barriers",barriesFClass, m_NAContext, 50);
            INASolver naSolver = m_NAContext.Solver;//创建网络分析对象
            try
            {
                naSolver.Solve(m_NAContext, gpMessages, null);//执行最短路径分析
                

            }
            catch(Exception ex) 
            {
                MessageBox.Show("未能找到有效路径"+ex.Message,"提示",
                    MessageBoxButtons.OKCancel,MessageBoxIcon.Warning);
                return;
            }
            //将“Routes”图层组下的“Stops”、“Point Barriers”图层设为不可见
            for (int i = 0; i < m_hookHelper.FocusMap.LayerCount; i++)
            {
                if (m_hookHelper.FocusMap.get_Layer(i).Name == m_NAContext.Solver.DisplayName)
                {
                    //ICompositeLayer Interface Provides access to members that work with a collection of layers that behaves like a single layer.
                    //CompositeLayer为图层组类型
                    ICompositeLayer pCompositeLayer = m_hookHelper.FocusMap.get_Layer(i) as ICompositeLayer;
                    for (int t = 0; t < pCompositeLayer.Count; t++)
                    {
                        ILayer pResultLayer = pCompositeLayer.get_Layer(t);
                        if (pResultLayer.Name == "Stops" || pResultLayer.Name == "Point Barriers")
                        {
                            pResultLayer.Visible = false;
                            continue;
                        }
                    }
                }
            }
           
            //接下来将地图的视图范围缩放至最短路径的显示范围
            IGeoDataset geoDataset;//地理数据集
            IEnvelope envelope;//最小边界矩形
            geoDataset = m_NAContext.NAClasses.get_ItemByName("Routes") as IGeoDataset;
            //The IGeoDataset::Extent property returns an envelope representing the maximum extent of data which has been stored in the dataset.
            envelope = geoDataset.Extent;
            if (!envelope.IsEmpty)
            {
                envelope.Expand(1.1, 1.1, true);
            }
            //将地图的显示的范围设置为“Routes”图层的数据范围
            m_hookHelper.ActiveView.Extent = envelope;
            //刷新视图
            m_hookHelper.ActiveView.Refresh();


            #region 显示路径详细信息
            //GetResults(naLayer);
            ITable table = m_NAContext.NAClasses.get_ItemByName("Routes") as ITable;
            //int count = table.RowCount(null);
            ICursor cursor = table.Search(null,false);
            IRow row = cursor.NextRow();
           
            //int wor = row.Fields.FieldCount;
            for (int i = 0; i < table.Fields.FieldCount; i++)
            {
                if (table.Fields.get_Field(i).AliasName == "Total_Shape_Length")
                {
                    MessageBox.Show("路径长度："+ row.get_Value(i) + "米","路径信息",
                        MessageBoxButtons.OK,MessageBoxIcon.None);
                }      
            }
                
            //IFeatureClass routesData = geoDataset as IFeatureClass;
            //IFeatureClass routes = m_NAContext.NAClasses.get_ItemByName("Routes") as IFeatureClass;
            //int counti = routes.FeatureCount(null);
            //IFeatureCursor featureCursor = routesData.Search(null, false);
            //IFeature shortPath = featureCursor.NextFeature();
            
            //string fieldName;
            //string aliaName;
            //for (int i = 0; i < feature.Fields.FieldCount; i++)
            //{
            //    fieldName = feature.Fields.get_Field(i).Name;
            //    aliaName = feature.Fields.get_Field(i).AliasName;
            //}

            #endregion
           
        }

        #endregion
        //该函数用于判断，图层是否存在于当前地图
        private bool IFLayerExist(string layerName)
        {
            for (int i = 0; i < m_hookHelper.FocusMap.LayerCount; i++)
            {
                if (m_hookHelper.FocusMap.get_Layer(i).Name == layerName)
                    return true;

            }
            return false;
        }
        
        }
    }


    



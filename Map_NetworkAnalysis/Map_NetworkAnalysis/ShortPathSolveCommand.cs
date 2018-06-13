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

        public static INAContext m_NAContext;   //�����������������
        private INetworkDataset networkDataset;//�����������ݼ�
        private IFeatureClass inputFClass;//����վ��Ҫ����
        private IFeatureClass barriesFClass;//�����ϰ���Ҫ����
        string path = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;//��ȡ�˳�������·��
        public ShortPathSolveCommand()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = "NetworkAnalyst"; //localizable text
            base.m_caption = "·�����";  //localizable text 
            base.m_message = "�õ����·��";  //localizable text
            base.m_toolTip = "·�����";  //localizable text
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
            string name = NetWorkAnalysClass.getPath(path) + "\\data\\HuanbaoGeodatabase.gdb";//�õ����ݿ��ļ�·��
            IFeatureWorkspace pFWorkspace = NetWorkAnalysClass.OpenWorkspace(name) as IFeatureWorkspace;//��Ҫ�ع����ռ�
            //"RouteNetwork","BaseData"�������ɸ���
            networkDataset = NetWorkAnalysClass.OpenPathNetworkDataset(pFWorkspace as IWorkspace,
                "RouteNetwork","BaseData");//���������ݼ�
            m_NAContext = NetWorkAnalysClass.CreatePathSolverContext(networkDataset);//ͨ���������ݼ������������������


            inputFClass = pFWorkspace.OpenFeatureClass("Stops");  //�������ִ�վ��Ҫ����
            barriesFClass = pFWorkspace.OpenFeatureClass("Barries");  //�������ִ��ϰ���Ҫ����
            if (IFLayerExist("NetworkDataset") == false)//��������NetworkDatasetͼ���ִ��
            {
                //������Ϊ��NetworkDataset����ͼ��
                ILayer layer;
                INetworkLayer networkLayer;
                networkLayer = new NetworkLayerClass();
                networkLayer.NetworkDataset = networkDataset;
                layer = networkLayer as ILayer;
                layer.Name = "NetworkDataset";
                m_hookHelper.ActiveView.FocusMap.AddLayer(layer);
                layer.Visible = false;
            }
            //����Ƿ������Ϊ��Route����ͼ�㡣��������ɾ��
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
            m_hookHelper.ActiveView.FocusMap.AddLayer(pLayer);//����ǰ��ͼ����Ӹ�ͼ��
            

            int featureCount = inputFClass.FeatureCount(null);//�õ�վ��Ҫ������Ҫ�صĸ���
            if (featureCount < 2)//�������������ܽ��з���
            {
                MessageBox.Show("ֻ��һ��վ�㣬���ܽ���·��������");
                return;
            }
            IGPMessages gpMessages = new GPMessagesClass();//����һ������������Ϣ���ض���
            //����վ��Ҫ�أ��������ݲ�
            NetWorkAnalysClass.LoadNANetworkLocations("Stops", inputFClass, m_NAContext, 80);
            //�����ϰ���Ҫ�أ��������ݲ�
            NetWorkAnalysClass.LoadNANetworkLocations("Barriers",barriesFClass, m_NAContext, 50);
            INASolver naSolver = m_NAContext.Solver;//���������������
            try
            {
                naSolver.Solve(m_NAContext, gpMessages, null);//ִ�����·������
                

            }
            catch(Exception ex) 
            {
                MessageBox.Show("δ���ҵ���Ч·��"+ex.Message,"��ʾ",
                    MessageBoxButtons.OKCancel,MessageBoxIcon.Warning);
                return;
            }
            //����Routes��ͼ�����µġ�Stops������Point Barriers��ͼ����Ϊ���ɼ�
            for (int i = 0; i < m_hookHelper.FocusMap.LayerCount; i++)
            {
                if (m_hookHelper.FocusMap.get_Layer(i).Name == m_NAContext.Solver.DisplayName)
                {
                    //ICompositeLayer Interface Provides access to members that work with a collection of layers that behaves like a single layer.
                    //CompositeLayerΪͼ��������
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
           
            //����������ͼ����ͼ��Χ���������·������ʾ��Χ
            IGeoDataset geoDataset;//�������ݼ�
            IEnvelope envelope;//��С�߽����
            geoDataset = m_NAContext.NAClasses.get_ItemByName("Routes") as IGeoDataset;
            //The IGeoDataset::Extent property returns an envelope representing the maximum extent of data which has been stored in the dataset.
            envelope = geoDataset.Extent;
            if (!envelope.IsEmpty)
            {
                envelope.Expand(1.1, 1.1, true);
            }
            //����ͼ����ʾ�ķ�Χ����Ϊ��Routes��ͼ������ݷ�Χ
            m_hookHelper.ActiveView.Extent = envelope;
            //ˢ����ͼ
            m_hookHelper.ActiveView.Refresh();


            #region ��ʾ·����ϸ��Ϣ
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
                    MessageBox.Show("·�����ȣ�"+ row.get_Value(i) + "��","·����Ϣ",
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
        //�ú��������жϣ�ͼ���Ƿ�����ڵ�ǰ��ͼ
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


    



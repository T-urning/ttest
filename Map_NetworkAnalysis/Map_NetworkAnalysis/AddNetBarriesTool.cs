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

namespace Map_NetworkAnalysis
{
    /// <summary>
    /// Summary description for AddNetBarriesTool.
    /// </summary>
    [Guid("8a72c0ad-a866-405e-9c7b-e941ec1ccb1f")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("Map_NetworkAnalysis.AddNetBarriesTool")]
    public sealed class AddNetBarriesTool : BaseTool
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

        private IFeatureWorkspace pFWorkspace;
        private IFeatureClass barriesFClass;
        string path = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

        public AddNetBarriesTool()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = "NetworkAnalyst"; //localizable text 
            base.m_caption = "添加障碍";  //localizable text 
            base.m_message = "鼠标在地图上单击即可";  //localizable text
            base.m_toolTip = "添加障碍";  //localizable text
            base.m_name = "AddBarriesTool";   //unique id, non-localizable (e.g. "MyCategory_MyTool")
            try
            {
                //
                // TODO: change resource name if necessary
                //
                string bitmapResourceName = GetType().Name + ".bmp";
                base.m_bitmap = new Bitmap(GetType(), bitmapResourceName);
                base.m_cursor = new System.Windows.Forms.Cursor(GetType(), GetType().Name + ".cur");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message, "Invalid Bitmap");
            }
        }

        #region Overridden Class Methods

        /// <summary>
        /// Occurs when this tool is created
        /// </summary>
        /// <param name="hook">Instance of the application</param>
        public override void OnCreate(object hook)
        {
            try
            {
                m_hookHelper = new HookHelperClass();
                m_hookHelper.Hook = hook;
                if (m_hookHelper.ActiveView == null)
                {
                    m_hookHelper = null;
                }
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
        /// Occurs when this tool is clicked
        /// </summary>
        public override void OnClick()
        {
            string name = NetWorkAnalysClass.getPath(path) + "\\data\\HuanbaoGeodatabase.gdb";
            //根据路径打开要素工作空间
            pFWorkspace = NetWorkAnalysClass.OpenWorkspace(name) as IFeatureWorkspace;
            //从工作空间里打开“Barries”要素类
            barriesFClass = pFWorkspace.OpenFeatureClass("Barries");
            //判断该要素类里是否含有要素，若有，则删除
            if (barriesFClass.FeatureCount(null) > 0)
            {
                ITable pTable = barriesFClass as ITable;
                pTable.DeleteSearchedRows(null);
            }

        }

        public override void OnMouseDown(int Button, int Shift, int X, int Y)
        {
            try {
                
                IPoint pBarriesPoint = new PointClass();
                //将鼠标在屏幕上点击的一点的坐标转换为地图上的坐标，并赋给pBarriesPoint
                pBarriesPoint = m_hookHelper.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(X,Y);
                IFeature newPointFeature = barriesFClass.CreateFeature();//创建新的障碍点要素
                try
                {
                    newPointFeature.Shape = pBarriesPoint;//将pStopsPoint赋给新要素的Shape属性
                }
                catch
                {
                    IGeometry pGeo = pBarriesPoint;
                    IZAware pZAware = pGeo as IZAware;
                    pZAware.ZAware = false;
                    newPointFeature.Shape = pGeo;
                }
                newPointFeature.Store();//存储障碍点要素
                //将障碍点图标添加到图层
                IGraphicsContainer pGrap = m_hookHelper.ActiveView as IGraphicsContainer;
                IColor pColor;
                IRgbColor pRgbColor = new RgbColorClass();
                pRgbColor.Red = 255;
                pRgbColor.Green = 255;
                pRgbColor.Blue = 255;
                pColor = pRgbColor as IColor;
                IPictureMarkerSymbol pms = new PictureMarkerSymbolClass();
                pms.BitmapTransparencyColor = pColor;
                string picturePath = NetWorkAnalysClass.getPath(path) + "\\data\\Img\\barries.bmp";
                //添加自定义障碍点图片
                pms.CreateMarkerSymbolFromFile(esriIPictureType.esriIPictureBitmap, picturePath);
                pms.Size = 18;
                IMarkerElement pMarkerEle = new MarkerElementClass();
                pMarkerEle.Symbol = pms as IMarkerSymbol;
                //将障碍点位置数据的空间参考设为与地图空间参考一致
                pBarriesPoint.SpatialReference = m_hookHelper.ActiveView.FocusMap.SpatialReference;
                IElement pEle = pMarkerEle as IElement;
                pEle.Geometry = pBarriesPoint;
                pGrap.AddElement(pEle, 1);
                m_hookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);

            }
            catch
            {
                MessageBox.Show("添加障碍点失败","提示",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                return;
            }
        }

        public override void OnMouseMove(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add AddNetBarriesTool.OnMouseMove implementation
        }

        public override void OnMouseUp(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add AddNetBarriesTool.OnMouseUp implementation
        }
        #endregion
    }
}

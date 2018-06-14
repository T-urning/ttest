using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.ADF;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.NetworkAnalyst;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.DataSourcesGDB;

namespace Map_NetworkAnalysis
{
    public sealed partial class MainForm : Form
    {
        #region class private members
        private IMapControl3 m_mapControl = null;
        private string m_mapDocumentName = string.Empty;
        #endregion
        private string path = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
        IFeatureLayer pTocFeatureLayer = null;//被选中的图层
        private FormAtrribute frmAttribute = null;//图层属性显示窗体
        #region class constructor
        public MainForm()
        {
            InitializeComponent();
        }
        #endregion

        private void MainForm_Load(object sender, EventArgs e)
        {
            //get the MapControl
            m_mapControl = (IMapControl3)axMapControl1.Object;

            //disable the Save menu (since there is no document yet)
            menuSaveDoc.Enabled = false;
        }

        #region Main Menu event handlers
        private void menuNewDoc_Click(object sender, EventArgs e)
        {
            //execute New Document command
            ICommand command = new CreateNewDocument();
            command.OnCreate(m_mapControl.Object);
            command.OnClick();
        }

        private void menuOpenDoc_Click(object sender, EventArgs e)
        {
            //execute Open Document command
            ICommand command = new ControlsOpenDocCommandClass();
            command.OnCreate(m_mapControl.Object);
            command.OnClick();
        }

        private void menuSaveDoc_Click(object sender, EventArgs e)
        {
            //execute Save Document command
            if (m_mapControl.CheckMxFile(m_mapDocumentName))
            {
                //create a new instance of a MapDocument
                IMapDocument mapDoc = new MapDocumentClass();
                mapDoc.Open(m_mapDocumentName, string.Empty);

                //Make sure that the MapDocument is not readonly
                if (mapDoc.get_IsReadOnly(m_mapDocumentName))
                {
                    MessageBox.Show("Map document is read only!");
                    mapDoc.Close();
                    return;
                }

                //Replace its contents with the current map
                mapDoc.ReplaceContents((IMxdContents)m_mapControl.Map);

                //save the MapDocument in order to persist it
                mapDoc.Save(mapDoc.UsesRelativePaths, false);

                //close the MapDocument
                mapDoc.Close();
            }
        }

        private void menuSaveAs_Click(object sender, EventArgs e)
        {
            //execute SaveAs Document command
            ICommand command = new ControlsSaveAsDocCommandClass();
            command.OnCreate(m_mapControl.Object);
            command.OnClick();
        }

        private void menuExitApp_Click(object sender, EventArgs e)
        {
            //exit the application
            Application.Exit();
        }
        #endregion

        //listen to MapReplaced evant in order to update the statusbar and the Save menu
        private void axMapControl1_OnMapReplaced(object sender, IMapControlEvents2_OnMapReplacedEvent e)
        {
            //get the current document name from the MapControl
            m_mapDocumentName = m_mapControl.DocumentFilename;

            //if there is no MapDocument, diable the Save menu and clear the statusbar
            if (m_mapDocumentName == string.Empty)
            {
                menuSaveDoc.Enabled = false;
                statusBarXY.Text = string.Empty;
            }
            else
            {
                //enable the Save manu and write the doc name to the statusbar
                menuSaveDoc.Enabled = true;
                statusBarXY.Text = System.IO.Path.GetFileName(m_mapDocumentName);
            }
           
        }

        private void axMapControl1_OnMouseMove(object sender, IMapControlEvents2_OnMouseMoveEvent e)
        {
            statusBarXY.Text = string.Format("{0}, {1}  {2}", e.mapX.ToString("#######.##"), e.mapY.ToString("#######.##"), axMapControl1.MapUnits.ToString().Substring(4));
        }

        private void axTOCControl1_OnMouseDown(object sender, ITOCControlEvents_OnMouseDownEvent e)
        {
            if (e.button == 2)
            {
                esriTOCControlItem pItem = esriTOCControlItem.esriTOCControlItemNone;
                IBasicMap pMap = null;
                object unk = null;
                object data = null;
                ILayer pLayer = null;
                axTOCControl1.HitTest(e.x, e.y, ref pItem, ref pMap, ref pLayer, ref unk, ref data);
                pTocFeatureLayer = pLayer as IFeatureLayer;
                if (pItem == esriTOCControlItem.esriTOCControlItemLayer && pTocFeatureLayer != null)
                {
                    contextMenuStrip2.Show(Control.MousePosition);
                }
            }
        }
        //加载站点
        private void addStopsMenuItem_Click(object sender, EventArgs e)
        {
            ICommand pCommand;
            pCommand = new AddNetStopsTool();//新建自定义的“添加站点”工具
            pCommand.OnCreate(axMapControl1.Object);//创建该工具
            axMapControl1.CurrentTool = pCommand as ITool;//将“添加站点”工具定义为地图控件的当前工具
            pCommand = null;
        }
        //加载障碍点
        private void addBarriesMenuItem_Click(object sender, EventArgs e)
        {
            ICommand pCommand;
            pCommand = new AddNetBarriesTool();//新建自定义的“添加障碍点”工具
            pCommand.OnCreate(axMapControl1.Object);//创建该工具
            axMapControl1.CurrentTool = pCommand as ITool;//将“添加障碍点”工具定义为地图控件的当前工具
            pCommand = null;
        }
        //最短路径分析
        private void routeSolverMenuItem_Click(object sender, EventArgs e)
        {
            ICommand pCommand;
            pCommand = new ShortPathSolveCommand();//新建自定义的“执行分析”命令
            pCommand.OnCreate(axMapControl1.Object);//创建该命令
            pCommand.OnClick();//启动该命令
            pCommand = null;
            #region 道路详细信息
            //获取最短路径的长度
            string totalLengthOfRoute = "";
            IFeatureLayer shortRouteLayer = NetWorkAnalysClass.GetLayerByName(
                m_mapControl.Map, "Routes") as IFeatureLayer;
            IFeatureCursor featureCursor = shortRouteLayer.Search(null, false);
            IFeature featue = featureCursor.NextFeature();
            for (int i = 0; i < featue.Fields.FieldCount; i++)
            {
                if (featue.Fields.get_Field(i).Name == "Total_Shape_Length")
                {
                    totalLengthOfRoute =  featue.get_Value(i).ToString();
                }
            }
            //获取与最短路径有关的道路的名字
            FormQueryBySpatial formQueryBySpatial = new FormQueryBySpatial();
            ArrayList  containRouteNameList = formQueryBySpatial.selectRouteNameBySpatial(
                esriSpatialRelEnum.esriSpatialRelContains, m_mapControl.Map);
            ArrayList overlapRouteNameList = formQueryBySpatial.selectRouteNameBySpatial(
                esriSpatialRelEnum.esriSpatialRelOverlaps, m_mapControl.Map);
            m_mapControl.Map.ClearSelection();
            string output = "经过道路有：";
            
            for (int i = 0; i < containRouteNameList.Count; i++) {
                output += containRouteNameList[i].ToString() + ","; 
            }
            output = output.Substring(0, output.Length - 1);
            output += "\n" + "站点所在道路为："; 
            for (int i = 0; i < overlapRouteNameList.Count; i++)
            {
                output += overlapRouteNameList[i].ToString() + ",";
            }
            output = output.Substring(0,output.Length-1);
            output = output + "\n" + "路径长度：" + totalLengthOfRoute + "米";
            MessageBox.Show(output);
            #endregion
        }
        private void clearResultMenuItem_Click(object sender, EventArgs e)
        {
            axMapControl1.CurrentTool = null;//将地图空间的当前工具注销
            try
            {
                string name = NetWorkAnalysClass.getPath(path) + "\\data\\HuanbaoGeodatabase.gdb";
                //打开工作空间
                IFeatureWorkspace pFWorkspace = NetWorkAnalysClass.OpenWorkspace(name) as IFeatureWorkspace;
                IGraphicsContainer pGrap = this.axMapControl1.ActiveView as IGraphicsContainer;
                pGrap.DeleteAllElements();//删除所添加的图片要素
                IFeatureClass inputFClass = pFWorkspace.OpenFeatureClass("Stops");//站点要素
                IFeatureClass barriesFClass = pFWorkspace.OpenFeatureClass("barries");//障碍点要素
                if (inputFClass.FeatureCount(null) > 0)
                {
                    ITable pTable = barriesFClass as ITable;
                    pTable.DeleteSearchedRows(null);
                }
                for (int i = 0; i < axMapControl1.LayerCount; i++) //删除分析结果
                {
                    ILayer pLayer = axMapControl1.get_Layer(i);
                    if (pLayer.Name == ShortPathSolveCommand.m_NAContext.Solver.DisplayName)
                    {
                        axMapControl1.DeleteLayer(i);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            this.axMapControl1.Refresh();
        }

        
      
        //属性查询菜单项点击事件响应函数
        private void queryAttributeMenuItem1_Click(object sender, EventArgs e)
        {
            //新建属性查询窗体
            FormQueryByAttribute formQueryByAttribute = new FormQueryByAttribute();
            //地图作为参数传入到窗体中
            formQueryByAttribute.CurrentMap = axMapControl1.Map;
            //显示窗体
            formQueryByAttribute.Show();
        }

        private void queyBySpatialRelMenuItem_Click(object sender, EventArgs e)
        {
            //新建空间查询窗体
            FormQueryBySpatial formQueryBySpatial = new FormQueryBySpatial();
             //地图作为参数传入到窗体中
            formQueryBySpatial.CurrentMap = axMapControl1.Map;
             //显示窗体
            formQueryBySpatial.Show();
        }

        private void clearSelectionMenuItem_Click(object sender, EventArgs e)
        {
            //清除选择集
            m_mapControl.Map.ClearSelection();
            //刷新视图
            m_mapControl.ActiveView.Refresh();
        }

       
        private void unDoMenuItem_Click(object sender, EventArgs e)
        {
            IExtentStack pExtentStack = m_mapControl.ActiveView.ExtentStack;
            //判断是否可以回到前一视图，第一个视图没有前视图
            if (pExtentStack.CanUndo())
            {
                pExtentStack.Undo();
                reDoMenuItem.Enabled = true;    //后一视图按钮可以使用
                if (!pExtentStack.CanUndo())
                {
                    unDoMenuItem.Enabled = false;   //前一视图按钮不能使用
                }
            }
            m_mapControl.ActiveView.Refresh();
        }

        private void reDoMenuItem_Click(object sender, EventArgs e)
        {
            //判断是否可以回到后一视图，最后一个视图没有后一视图
            IExtentStack pExtentStack = m_mapControl.ActiveView.ExtentStack;
            if (pExtentStack.CanRedo())
            {
                pExtentStack.Redo();
                unDoMenuItem.Enabled = true;    //前一视图按钮可以使用
                if (!pExtentStack.CanRedo())
                {
                    reDoMenuItem.Enabled = false;
                }
            }
            m_mapControl.ActiveView.Refresh();
        }
        //右键图层列表查看图层属性
        private void showFTLayerAttributeMenuItem_Click(object sender, EventArgs e)
        {
            if (frmAttribute == null || frmAttribute.IsDisposed)
            {
                frmAttribute = new FormAtrribute(axMapControl1.Map);
            }
            frmAttribute.CurFeatureLayer = pTocFeatureLayer;
            frmAttribute.InitUI();
            frmAttribute.ShowDialog();
        }
        //右键图层列表缩放至图层
        private void zoomToLayerMenuItem_Click(object sender, EventArgs e)
        {
            if (pTocFeatureLayer == null) return;
            (axMapControl1.Map as IActiveView).Extent = pTocFeatureLayer.AreaOfInterest;
            (axMapControl1.Map as IActiveView).PartialRefresh(
                esriViewDrawPhase.esriViewGeography, null, null);
        }
        //右键图层列表删除图层
        private void deleteLayerMenuItem_Click(object sender, EventArgs e)
        {
            if (pTocFeatureLayer == null) return;
            DialogResult result = MessageBox.Show("是否删除[" + pTocFeatureLayer.Name + "]图层",
                "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (result == DialogResult.OK)
            {
                axMapControl1.Map.DeleteLayer(pTocFeatureLayer);

            }
            axMapControl1.ActiveView.Refresh();
        }

       

        

        
    }
}
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

        }
        //����վ��
        private void addStopsMenuItem_Click(object sender, EventArgs e)
        {
            ICommand pCommand;
            pCommand = new AddNetStopsTool();//�½��Զ���ġ����վ�㡱����
            pCommand.OnCreate(axMapControl1.Object);//�����ù���
            axMapControl1.CurrentTool = pCommand as ITool;//�������վ�㡱���߶���Ϊ��ͼ�ؼ��ĵ�ǰ����
            pCommand = null;
        }
        //�����ϰ���
        private void addBarriesMenuItem_Click(object sender, EventArgs e)
        {
            ICommand pCommand;
            pCommand = new AddNetBarriesTool();//�½��Զ���ġ�����ϰ��㡱����
            pCommand.OnCreate(axMapControl1.Object);//�����ù���
            axMapControl1.CurrentTool = pCommand as ITool;//��������ϰ��㡱���߶���Ϊ��ͼ�ؼ��ĵ�ǰ����
            pCommand = null;
        }
        //���·������
        private void routeSolverMenuItem_Click(object sender, EventArgs e)
        {
            ICommand pCommand;
            pCommand = new ShortPathSolveCommand();//�½��Զ���ġ�ִ�з���������
            pCommand.OnCreate(axMapControl1.Object);//����������
            pCommand.OnClick();//����������
            pCommand = null;
        }
        private void clearResultMenuItem_Click(object sender, EventArgs e)
        {
            axMapControl1.CurrentTool = null;//����ͼ�ռ�ĵ�ǰ����ע��
            try
            {
                string name = NetWorkAnalysClass.getPath(path) + "\\data\\HuanbaoGeodatabase.gdb";
                //�򿪹����ռ�
                IFeatureWorkspace pFWorkspace = NetWorkAnalysClass.OpenWorkspace(name) as IFeatureWorkspace;
                IGraphicsContainer pGrap = this.axMapControl1.ActiveView as IGraphicsContainer;
                pGrap.DeleteAllElements();//ɾ������ӵ�ͼƬҪ��
                IFeatureClass inputFClass = pFWorkspace.OpenFeatureClass("Stops");//վ��Ҫ��
                IFeatureClass barriesFClass = pFWorkspace.OpenFeatureClass("barries");//�ϰ���Ҫ��
                if (inputFClass.FeatureCount(null) > 0)
                {
                    ITable pTable = barriesFClass as ITable;
                    pTable.DeleteSearchedRows(null);
                }
                for (int i = 0; i < axMapControl1.LayerCount; i++) //ɾ���������
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

        private void axMapControl1_OnMouseDown(object sender, IMapControlEvents2_OnMouseDownEvent e)
        {

        }

      
        //���Բ�ѯ�˵������¼���Ӧ����
        private void queryAttributeMenuItem1_Click(object sender, EventArgs e)
        {
            //�½����Բ�ѯ����
            FormQueryByAttribute formQueryByAttribute = new FormQueryByAttribute();
            //��ͼ��Ϊ�������뵽������
            formQueryByAttribute.CurrentMap = axMapControl1.Map;
            //��ʾ����
            formQueryByAttribute.Show();
        }

        private void queyBySpatialRelMenuItem_Click(object sender, EventArgs e)
        {
            //�½��ռ��ѯ����
            FormQueryBySpatial formQueryBySpatial = new FormQueryBySpatial();
             //��ͼ��Ϊ�������뵽������
            formQueryBySpatial.CurrentMap = axMapControl1.Map;
             //��ʾ����
            formQueryBySpatial.Show();
        }

        private void clearSelectionMenuItem_Click(object sender, EventArgs e)
        {
            //���ѡ��
            m_mapControl.Map.ClearSelection();
            //ˢ����ͼ
            m_mapControl.ActiveView.Refresh();
        }

        private void axToolbarControl1_OnMouseDown(object sender, IToolbarControlEvents_OnMouseDownEvent e)
        {

        }

        private void unDoMenuItem_Click(object sender, EventArgs e)
        {
            IExtentStack pExtentStack = m_mapControl.ActiveView.ExtentStack;
            //�ж��Ƿ���Իص�ǰһ��ͼ����һ����ͼû��ǰ��ͼ
            if (pExtentStack.CanUndo())
            {
                pExtentStack.Undo();
                reDoMenuItem.Enabled = true;    //��һ��ͼ��ť����ʹ��
                if (!pExtentStack.CanUndo())
                {
                    unDoMenuItem.Enabled = false;   //ǰһ��ͼ��ť����ʹ��
                }
            }
            m_mapControl.ActiveView.Refresh();
        }

        private void reDoMenuItem_Click(object sender, EventArgs e)
        {
            //�ж��Ƿ���Իص���һ��ͼ�����һ����ͼû�к�һ��ͼ
            IExtentStack pExtentStack = m_mapControl.ActiveView.ExtentStack;
            if (pExtentStack.CanRedo())
            {
                pExtentStack.Redo();
                unDoMenuItem.Enabled = true;    //ǰһ��ͼ��ť����ʹ��
                if (!pExtentStack.CanRedo())
                {
                    reDoMenuItem.Enabled = false;
                }
            }
            m_mapControl.ActiveView.Refresh();
        }

        

        
    }
}
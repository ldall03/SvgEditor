using FinalProject.Controls;
using FinalProject.Controllers;
using Svg;

namespace FinalProject
{
    public enum Mode
    {
        Select,
        DrawRect,
        DrawSquare,
        DrawEllipse,
        DrawCircle
    }

    public partial class Form1 : Form
    {
        public ShapeDrawingController ShapeDrawingController { get; }
        public FileController FileController { get; }
        public UndoRedoController UndoRedoController { get; }
        public SelectionController SelectionController { get; }
        public LayerController LayerController { get; private set; }
        public DocumentModel DocumentModel { get; private set; }

        public Mode CurrentMode { get; private set; }

        private Button _selected_tool_btn;
        private bool _shift_down = false;
        private bool _fire_on_change_event = true; // To not fire NumberBoxOnChange event when filling the table

        public Form1()
        {
            InitializeComponent();
            DocumentModel = new DocumentModel();
            ShapeDrawingController = new ShapeDrawingController(pictureBox1);
            FileController = new FileController(OnSave, OnOpen);
            SelectionController = new SelectionController();
            LayerController = new LayerController();

            CurrentMode = Mode.DrawRect;
            _selected_tool_btn = rectToolButton;
            _selected_tool_btn.BackColor = Color.Gray;

            KeyPreview = true;
            KeyDown += new KeyEventHandler(onKeyDown);

            // Styling
            fillColorPanel.BackColor = Color.Red;
            borderColorPanel.BackColor = Color.Black;

            tabControl1.Appearance = TabAppearance.FlatButtons;
            tabControl1.ItemSize = new Size(0, 1);
            tabControl1.SizeMode = TabSizeMode.Fixed;
            foreach (TabPage tab in tabControl1.TabPages)
                tab.Text = "";
            tabControl2.Appearance = TabAppearance.FlatButtons;
            tabControl2.ItemSize = new Size(0, 1);
            tabControl2.SizeMode = TabSizeMode.Fixed;
            foreach (TabPage tab in tabControl2.TabPages)
                tab.Text = "";
            tabControl2.SelectTab(1);

            var layer = new LayerControl("Layer_0");
            layer.onLayerSelected += layerControl_OnSelect;
            layer.onToggleVisible += layerControl_Visible;
            layer.onSendUp += layerControl_SendUp;
            layer.onSendDown += layerControl_SendDown;
            layer.onRemove += layerControl_Remove;
            LayerController.AddLayer(layer);
            layers_Container.Controls.Add(layer);
            DocumentModel.Layers.Add(layer.LayerModel);

            UndoRedoController = new UndoRedoController(DocumentModel.Clone());
        }
        public void OnSave(string fileName)
        {
            var json = DocumentModel.ToJson();
            File.WriteAllText(fileName, json);
        }

        public void OnOpen(string fileName)
        {
            var text = File.ReadAllText(fileName);
            DocumentModel = DocumentModel.FromJson(text);
            LayerController = new LayerController(DocumentModel.Layers);
            foreach (var layer in LayerController.LayerControls)
            {
                layer.onLayerSelected += layerControl_OnSelect;
                layer.onToggleVisible += layerControl_Visible;
                layer.onSendUp += layerControl_SendUp;
                layer.onSendDown += layerControl_SendDown;
                layer.onRemove += layerControl_Remove;
            }

            UpdateDocument();
            UpdateLayerChange();
        }

        public void UpdateDocument()
        {
            FileController.Modified = true;
            pictureBox1.Invalidate();
        }

        public void StartDrawingShape(SimpleShapeModel shape)
        {
            if (LayerController.CurrentLayer.IsLocked || LayerController.LayerControls.Count == 0 || LayerController.CurrentLayer == null)
                return;

            ShapeDrawingController.StartDrawing(shape, MousePosition);
            LayerController.AddShape(shape);
        }

        public void CancelDrawingShape()
        {
            if (!ShapeDrawingController.IsDrawing())
                return;

            LayerController.RemoveShape(ShapeDrawingController.Shape);
            ShapeDrawingController.StopDrawing();
            UpdateDocument();
        }

        public void UpdateShape()
        {
            if (!ShapeDrawingController.IsDrawing())
                return;
            ShapeDrawingController.Update(MousePosition);
            UpdateDocument();
        }

        public void CompleteDrawingShape()
        {
            if (!ShapeDrawingController.IsDrawing())
                return;
            ShapeDrawingController.StopDrawing();
            UndoRedoController.Add(DocumentModel.Clone());
            UpdateDocument();
        }

        public bool HitTest(SimpleShapeModel shape, Point pt)
        {
                return (pt.X >= shape.Left && pt.X <= shape.Right)
                       && (pt.Y >= shape.Top && pt.Y <= shape.Bottom);
        }

        public SimpleShapeModel? HitTest(Point pt)
        {
            foreach (var x in LayerController.GetElements().Reverse())
                if (HitTest(x, pt))
                    return x;

            return null;
        }
        
        public void SelectWhereMouse()
        {
            var e = HitTest(MousePositionRelativeToPicture());
            if (!_shift_down)
                SelectionController.ClearSelection();

            if (e != null)
            {
                SelectionController.UpdateSelection(e);
                FillSelectedInfo(SelectionController.LastSelected);
            }
            else
            {
                tabControl2.SelectTab(1);
            }
            UpdateDocument();
        }

        public void FillSelectedInfo(SimpleShapeModel? shape)
        {
            if (shape == null)
            {
                tabControl2.SelectTab(1);
                return;
            }
            _fire_on_change_event = false;
            x_NumberBox.Value = (int)shape.Position.X;
            y_NumberBox.Value = (int)shape.Position.Y;
            width_NumberBox.Value = (int)shape.Width;
            height_NumberBox.Value = (int)shape.Height;
            borderWidth_NumberBox.Value = (int)shape.StrokeWidth;
            shapeFillPanel.BackColor = shape.FillColor.ToDrawingColor();
            shapeBorder_Panel.BackColor = shape.StrokeColor.ToDrawingColor();
            zindex_NumberBox.Value = shape.ZIndex;
            radiusNumberBox.Value = (int)shape.BorderRadius;
            angleNumberBox.Value = shape.Angle;
            shapeFillBox.Checked = shape.Filled;
            _fire_on_change_event = true;
            tabControl2.SelectTab(0);
        }

        public void DeleteSelected(object sender, EventArgs e) // TODO bug with undo
        {
            foreach (var s in SelectionController.SelectedShapes)
                LayerController.RemoveShape(s);

            SelectionController.ClearSelection();
            UndoRedoController.Add(DocumentModel.Clone());
            UpdateDocument();
        }

        public Point MousePositionRelativeToPicture()
        {
            var pt = pictureBox1.PointToScreen(Point.Empty);
            return new Point(MousePosition.X - pt.X, MousePosition.Y - pt.Y);
        }

        // Modes
        public void selectToolButton_Click(object sender, EventArgs e)
        {
            CurrentMode = Mode.Select;
            SelectButton(selectToolButton);
            tabControl1.SelectTab(1);
        }

        public void rectangleToolButton_Click(object sender, EventArgs e)
        {
            CurrentMode = Mode.DrawRect;
            SelectionController.ClearSelection();
            SelectButton(rectToolButton);
            tabControl1.SelectTab(0);
            tabControl2.SelectTab(1);
            UpdateDocument();
        }
        public void squareToolButton_Click(object sender, EventArgs e)
        {
            CurrentMode = Mode.DrawSquare;
            SelectionController.ClearSelection();
            SelectButton(squareToolButton);
            tabControl1.SelectTab(0);
            tabControl2.SelectTab(1);
            UpdateDocument();
        }
        public void ellipseToolButton_Click(object sender, EventArgs e)
        {
            CurrentMode = Mode.DrawEllipse;
            SelectionController.ClearSelection();
            SelectButton(ellipseToolButton);
            tabControl1.SelectTab(0);
            tabControl2.SelectTab(1);
            UpdateDocument();
        }
        public void circleToolButton_Click(object sender, EventArgs e)
        {
            CurrentMode = Mode.DrawCircle;
            SelectionController.ClearSelection();
            SelectButton(circleToolButton);
            tabControl1.SelectTab(0);
            tabControl2.SelectTab(1);
            UpdateDocument();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                switch (CurrentMode)
                {
                    case Mode.DrawRect:
                        StartDrawingShape(new RectangleModel()
                        {
                            FillColor = fillColDlg.Color.ToColorModel(),
                            CurrentStrokeColor = borderColDlg.Color.ToColorModel(),
                            StrokeColor = borderColDlg.Color.ToColorModel(),
                            StrokeWidth = borderWidthBar.Value,
                            Filled = filled_CheckBox.Checked,
                            BorderRadius = radiusBar.Value
                        });
                        break;
                    case Mode.DrawSquare:
                        StartDrawingShape(new SquareModel()
                        {
                            FillColor = fillColDlg.Color.ToColorModel(),
                            CurrentStrokeColor = borderColDlg.Color.ToColorModel(),
                            StrokeColor = borderColDlg.Color.ToColorModel(),
                            StrokeWidth = borderWidthBar.Value,
                            Filled = filled_CheckBox.Checked,
                            BorderRadius = radiusBar.Value
                        });
                        break;
                    case Mode.DrawEllipse:
                        StartDrawingShape(new EllipseModel()
                        {
                            FillColor = fillColDlg.Color.ToColorModel(),
                            CurrentStrokeColor = borderColDlg.Color.ToColorModel(),
                            StrokeColor = borderColDlg.Color.ToColorModel(),
                            StrokeWidth = borderWidthBar.Value,
                            Filled = filled_CheckBox.Checked
                        });
                        break;
                    case Mode.DrawCircle:
                        StartDrawingShape(new CircleModel()
                        {
                            FillColor = fillColDlg.Color.ToColorModel(),
                            CurrentStrokeColor = borderColDlg.Color.ToColorModel(),
                            StrokeColor = borderColDlg.Color.ToColorModel(),
                            StrokeWidth = borderWidthBar.Value,
                            Filled = filled_CheckBox.Checked
                        });
                        break;
                    case Mode.Select:
                        SelectWhereMouse();
                        break;
                }
            }
            if (e.Button == MouseButtons.Right)
            {
                CancelDrawingShape();
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (ShapeDrawingController.IsDrawing())
                UpdateShape(); 
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            CompleteDrawingShape();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            var svgDoc = DocumentModel.ToSvg();
            svgDoc.Draw(e.Graphics);
        }

        private void saveMenuButton_Click(object sender, EventArgs e)
        {
            FileController.SaveCurrentFile();
        }

        private void saveasMenuButton_Click(object sender, EventArgs e)
        {
            FileController.SaveAs();
        }

        private void newMenuButton_Click(object sender, EventArgs e)
        {
            if (!FileController.NewWithCheck())
                return;

            DocumentModel = new DocumentModel();
            UpdateDocument();
        }

        private void OpenMenuButton_Click(object sender, EventArgs e)
        {
            FileController.Open();
        }

        private void exportButton_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "SVG files (*.svg)|*.svg|All files (*.*)|*.*";
            if (saveFileDialog1.ShowDialog() != DialogResult.OK)
                return;
            File.WriteAllText(saveFileDialog1.FileName, DocumentModel.ToSvg().GetXML());
        }

        private void undoButton_Click(object sender, EventArgs e)
        {
            var doc = UndoRedoController.Undo();
            if (doc != null)
            {
                DocumentModel = doc;
                LayerController = new LayerController(DocumentModel.Layers);
                foreach (var layer in LayerController.LayerControls)
                {
                    layer.onLayerSelected += layerControl_OnSelect;
                    layer.onToggleVisible += layerControl_Visible;
                    layer.onSendUp += layerControl_SendUp;
                    layer.onSendDown += layerControl_SendDown;
                    layer.onRemove += layerControl_Remove;
                }
                UpdateDocument();
                UpdateLayerChange();
            }
        }

        private void RedoButton_Click(object sender, EventArgs e)
        {
            var doc = UndoRedoController.Redo();
            if (doc != null)
            {
                DocumentModel = doc;
                LayerController = new LayerController(DocumentModel.Layers);
                foreach (var layer in LayerController.LayerControls)
                {
                    layer.onLayerSelected += layerControl_OnSelect;
                    layer.onToggleVisible += layerControl_Visible;
                    layer.onSendUp += layerControl_SendUp;
                    layer.onSendDown += layerControl_SendDown;
                    layer.onRemove += layerControl_Remove;
                }
                UpdateDocument();
                UpdateLayerChange();
            }
        }

        private void SelectButton(Button btn)
        {
            _selected_tool_btn.BackColor = Color.White;
            btn.BackColor = Color.Gray;
            _selected_tool_btn = btn;
        }

        private void fillColorPanel_Click(object sender, EventArgs e)
        {
            if (fillColDlg.ShowDialog() == DialogResult.OK)
                        fillColorPanel.BackColor = fillColDlg.Color;
        }

        private void borderColorPanel_Click(object sender, EventArgs e)
        {
            if (borderColDlg.ShowDialog() == DialogResult.OK)
                        borderColorPanel.BackColor = borderColDlg.Color;
        }

        // Shape property table
        private void xNumberBox_OnChange(object sender, EventArgs e)
        {
            if (!_fire_on_change_event) return;

            foreach (var shape in SelectionController.SelectedShapes)
                shape.Position.X = (float)x_NumberBox.Value;
            UndoRedoController.Add(DocumentModel.Clone());
            UpdateDocument();
        }

        private void yNumberBox_OnChange(object sender, EventArgs e)
        {
            if (!_fire_on_change_event) return;

            foreach (var shape in SelectionController.SelectedShapes)
                shape.Position.Y = (float)y_NumberBox.Value;
            UndoRedoController.Add(DocumentModel.Clone());
            UpdateDocument();
        }
        private void widthNumberBox_OnChange(object sender, EventArgs e)
        {
            if (!_fire_on_change_event) return;

            foreach (var shape in SelectionController.SelectedShapes)
                shape.Size.Width = (float)width_NumberBox.Value;
            UndoRedoController.Add(DocumentModel.Clone());
            UpdateDocument();
        }
        private void heightNumberBox_OnChange(object sender, EventArgs e)
        {
            if (!_fire_on_change_event) return;

            foreach (var shape in SelectionController.SelectedShapes)
                shape.Size.Height = (float)height_NumberBox.Value;
            UndoRedoController.Add(DocumentModel.Clone());
            UpdateDocument();
        }

        private void shapeFillPanel_OnClick(object sender, EventArgs e)
        {
            if (!_fire_on_change_event) return;

            var dlg = new ColorDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                shapeFillPanel.BackColor = dlg.Color;
                foreach (var shape in SelectionController.SelectedShapes)
                    shape.FillColor = dlg.Color.ToColorModel();
            }
            UndoRedoController.Add(DocumentModel.Clone());
            UpdateDocument();
        }
        
        private void shapeBorderPanel_OnClick(object sender, EventArgs e)
        {
            if (!_fire_on_change_event) return;

            var dlg = new ColorDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                shapeBorder_Panel.BackColor = dlg.Color;
                foreach (var shape in SelectionController.SelectedShapes)
                    shape.StrokeColor = dlg.Color.ToColorModel();
            }
            UndoRedoController.Add(DocumentModel.Clone());
            UpdateDocument();
        }
        private void borderWidthNumberBox_OnChange(object sender, EventArgs e)
        {
            if (!_fire_on_change_event) return;

            foreach (var shape in SelectionController.SelectedShapes)
                shape.StrokeWidth = (float)borderWidth_NumberBox.Value;
            UndoRedoController.Add(DocumentModel.Clone());
            UpdateDocument();
        }

        private void zindexNumberBox_OnChange(object sender, EventArgs e)
        {
            if (!_fire_on_change_event) return;

            foreach (var shape in SelectionController.SelectedShapes)
                shape.ZIndex = (int)zindex_NumberBox.Value;
            LayerController.Sort();
            UndoRedoController.Add(DocumentModel.Clone());
            UpdateDocument();
        }

        private void radiusNumberBox_Changed(object sender, EventArgs e)
        {
            if (!_fire_on_change_event) return;

            foreach (var shape in SelectionController.SelectedShapes)
                shape.BorderRadius = (int)radiusNumberBox.Value;
            UndoRedoController.Add(DocumentModel.Clone());
            UpdateDocument();
        }

        private void angleNumberBox_Changed(object sender, EventArgs e)
        {
            if (!_fire_on_change_event) return;

            foreach (var shape in SelectionController.SelectedShapes)
                shape.Angle = (int)angleNumberBox.Value;
            UndoRedoController.Add(DocumentModel.Clone());
            UpdateDocument();
        }

        private void shapeFillBox_OnChange(object sender, EventArgs e)
        {
            if (!_fire_on_change_event) return;

            foreach (var shape in SelectionController.SelectedShapes)
                shape.Filled = shapeFillBox.Checked;
            UndoRedoController.Add(DocumentModel.Clone());
            UpdateDocument();
        }

        private void onKeyDown(object sender, KeyEventArgs e)
        {
            if (!_fire_on_change_event) return;

            if (e.KeyCode == Keys.ShiftKey)
                _shift_down = true;
        }

        private void onKeyUp(object sender, KeyEventArgs e)
        {
            if (!_fire_on_change_event) return;

            if (e.KeyCode == Keys.ShiftKey)
                _shift_down = false;
        }

        // Layer stuff
        private void layerButton_Click(object sender, EventArgs e)
        {
            SelectionController.ClearSelection();
            tabControl2.SelectTab(1);
            pictureBox1.Invalidate();

            var count = LayerController.LayerControls.Count;
            var name = $"layer_{count}";
            var layer = new LayerControl(name);
            layer.onLayerSelected += layerControl_OnSelect;
            layer.onToggleVisible += layerControl_Visible;
            layer.onToggleLocked += layerControl_Locked;
            layer.onSendUp += layerControl_SendUp;
            layer.onSendDown += layerControl_SendDown;
            layer.onRemove += layerControl_Remove;
            layers_Container.Controls.Add(layer);
            LayerController.AddLayer(layer);
            DocumentModel.Layers.Add(layer.LayerModel);
            UndoRedoController.Add(DocumentModel.Clone());
        }

        private void layerControl_OnSelect(object? sender, EventArgs e)
        {
            LayerController.SetCurrentLayer((LayerControl)sender);
            SelectionController.ClearSelection();
            tabControl2.SelectTab(1);
            pictureBox1.Invalidate();
        }

        private void layerControl_Visible(object? sender, EventArgs e)
        {
            pictureBox1.Invalidate();
            UndoRedoController.Add(DocumentModel.Clone());
        }

        private void layerControl_Locked(object? sender, EventArgs e)
        {
            UndoRedoController.Add(DocumentModel.Clone());
        }

        private void layerControl_SendUp(object? sender, EventArgs e)
        {
            LayerController.MoveUp((LayerControl)sender, DocumentModel.Layers);
            UpdateLayerChange();
            UpdateDocument();
            UndoRedoController.Add(DocumentModel.Clone());
        }

        private void layerControl_SendDown(object? sender, EventArgs e)
        {
            LayerController.MoveDown((LayerControl)sender, DocumentModel.Layers);
            UpdateLayerChange();
            UpdateDocument();
            UndoRedoController.Add(DocumentModel.Clone());
        }

        private void layerControl_Remove(object? sender, EventArgs e)
        {
            LayerController.RemoveLayer((LayerControl)sender, DocumentModel.Layers);
            UpdateLayerChange();
            UpdateDocument();
            UndoRedoController.Add(DocumentModel.Clone());
        }

        private void UpdateLayerChange()
        {
            while (layers_Container.Controls.Count > 1)
                layers_Container.Controls.RemoveAt(1);

            foreach (var layer in LayerController.LayerControls)
                layers_Container.Controls.Add(layer);
        }
    }
}
namespace FinalProject.Controllers
{
    public class SelectionController
    {
        public List<SimpleShapeModel> SelectedShapes { get; private set; } = new();
        public SimpleShapeModel? LastSelected { get; private set; }

        public void ClearSelection()
        {
            foreach (var shape in SelectedShapes)
                shape.CurrentStrokeColor = shape.StrokeColor;
            SelectedShapes.Clear();
            LastSelected = null;
        }

        public void UpdateSelection(SimpleShapeModel shape)
        {
            if (SelectedShapes.Contains(shape))
            {
                shape.CurrentStrokeColor = shape.StrokeColor;
                RemoveFromSelection(shape);
            }
            else
            {
                shape.CurrentStrokeColor = ColorModel.Red;
                SelectedShapes.Add(shape);
                LastSelected = shape;
            }
        }

        private void RemoveFromSelection(SimpleShapeModel shape)
        {
            SelectedShapes.Remove(shape);
            LastSelected = (SelectedShapes.Count > 0) ? SelectedShapes[SelectedShapes.Count - 1] : null;
        }
    }
}

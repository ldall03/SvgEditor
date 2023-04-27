using FinalProject.Controls;

namespace FinalProject.Controllers
{
    public class LayerController
    {
        public List<LayerControl> LayerControls { get; private set; } = new();
        public LayerControl? CurrentLayer { get; set; }

        public LayerController() { }

        // To initialize an instance and fill it with LayerModels
        public LayerController(List<LayerModel> layers)
        {
            foreach (var layer in layers)
            {
                var control = new LayerControl(layer.LayerName);
                control.LayerModel = layer;
                LayerControls.Add(control);
            }

            CurrentLayer = (LayerControls.Count > 0) ? LayerControls[0] : null;
            if (CurrentLayer != null)
                CurrentLayer.BorderStyle = BorderStyle.FixedSingle;
        }
        
        public void MoveUp(LayerControl layer, List<LayerModel> layers)
        {
            if (LayerControls[0] == layer) return;

            var i = 0;
            var l_index = 0;
            foreach (var l in LayerControls) // Find the index of the layer
            {
                if (l == layer) 
                    l_index = i;
                i++;
            }

            // Swap the layer in LayerControls and also in the list layers
            // as the LayerModels must have the same position as its LayerControl
            var tmpC = LayerControls[l_index];
            LayerControls[l_index] = LayerControls[l_index - 1];
            LayerControls[l_index - 1] = tmpC;

            var tmpM = layers[l_index];
            layers[l_index] = layers[l_index - 1];
            layers[l_index - 1] = tmpM;
        }

        public void MoveDown(LayerControl layer, List<LayerModel> layers)
        {
            if (LayerControls[LayerControls.Count - 1] == layer) return;

            var i = 0;
            var l_index = 0;
            foreach (var l in LayerControls) // Find the index of the layer

            {
                if (l == layer)
                    l_index = i;
                i++;
            }

            // Swap the layer in LayerControls and also in the list layers
            // as the LayerModels must have the same position as its LayerControl
            var tmpC = LayerControls[l_index];
            LayerControls[l_index] = LayerControls[l_index + 1];
            LayerControls[l_index + 1] = tmpC;

            var tmpM = layers[l_index];
            layers[l_index] = layers[l_index + 1];
            layers[l_index + 1] = tmpM;
        }

        public void SetCurrentLayer(LayerControl layer)
        {
            if (CurrentLayer != null)
                CurrentLayer.BorderStyle = BorderStyle.None;
            CurrentLayer = layer;
            CurrentLayer.BorderStyle = BorderStyle.FixedSingle;
        }

        public void AddLayer(LayerControl layer)
        {
            SetCurrentLayer(layer);
            LayerControls.Add(layer);
        }

        public void RemoveLayer(LayerControl layer, List<LayerModel> layer_models)
        {
            LayerControls.Remove(layer);
            layer_models.Remove(layer.LayerModel);
            if (LayerControls.Count > 0)
                SetCurrentLayer(LayerControls[0]);
            else
                CurrentLayer = null;
        }

        // Adds a shape to the current layer
        public void AddShape(SimpleShapeModel shape)
        {
            CurrentLayer.LayerModel.Shapes.Add(shape);
        }

        public void RemoveShape(SimpleShapeModel shape)
        {
            CurrentLayer.LayerModel.Shapes.Remove(shape);
        }

        // Rearrange the order for z-index
        public void Sort()
        {
            CurrentLayer.LayerModel.Sort();
        }

        public IEnumerable<SimpleShapeModel> GetElements()
        {
            foreach (var model in CurrentLayer.LayerModel.Shapes)
                yield return model;
        }
    }
}

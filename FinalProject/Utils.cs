using Svg;
using Svg.Transforms;

namespace FinalProject
{
    public static class Utils
    {
        public static Color ToDrawingColor(this ColorModel color)
        {
            return Color.FromArgb(color.R, color.G, color.B);
        }

        public static ColorModel ToColorModel(this Color color)
        {
            return new ColorModel() { R = color.R, G = color.G, B = color.B };
        }

        public static SvgPaintServer ToSvg(this ColorModel color)
        {
            return new SvgColourServer(color.ToDrawingColor());
        }

        public static SvgElement? ToSvg(this SimpleShapeModel shape)
        {
            switch (shape)
            {
                case RectangleModel rectangle:
                    return new SvgRectangle()
                    {
                        X = (float)rectangle.Left,
                        Y = (float)rectangle.Top,
                        Width = (float)rectangle.Width,
                        Height = (float)rectangle.Height,
                        Stroke = rectangle.CurrentStrokeColor.ToSvg(),
                        StrokeWidth = (float)rectangle.StrokeWidth,
                        Fill = rectangle.Filled ? rectangle.FillColor.ToSvg() : SvgPaintServer.None,
                        CornerRadiusX = (float)rectangle.BorderRadius,
                        CornerRadiusY = (float)rectangle.BorderRadius,
                        Transforms = new SvgTransformCollection() 
                            { new SvgRotate(rectangle.Angle, (float)rectangle.CenterX, (float)rectangle.CenterY) }
                    };
                case SquareModel square:
                    return new SvgRectangle()
                    {
                        X = (float)square.Left,
                        Y = (float)square.Top,
                        Width = (float)square.Radius * 2,
                        Height = (float)square.Radius * 2,
                        Stroke = square.CurrentStrokeColor.ToSvg(),
                        StrokeWidth = (float)square.StrokeWidth,
                        Fill = square.Filled ? square.FillColor.ToSvg() : SvgPaintServer.None,
                        CornerRadiusX = (float)square.BorderRadius,
                        CornerRadiusY = (float)square.BorderRadius,
                        Transforms = new SvgTransformCollection() 
                            { new SvgRotate(square.Angle, (float)square.CenterX, (float)square.CenterY) }
                    };
                case EllipseModel ellipse:
                    return new SvgEllipse()
                    {
                        CenterX = (float)ellipse.CenterX,
                        CenterY = (float)ellipse.CenterY,
                        RadiusX = (float)ellipse.Width / 2,
                        RadiusY = (float)ellipse.Height / 2,
                        Stroke = ellipse.CurrentStrokeColor.ToSvg(),
                        StrokeWidth = (float)ellipse.StrokeWidth,
                        Fill = ellipse.Filled ? ellipse.FillColor.ToSvg() : SvgPaintServer.None,
                        Transforms = new SvgTransformCollection() 
                            { new SvgRotate(ellipse.Angle, (float)ellipse.CenterX, (float)ellipse.CenterY) }
                    };
                case CircleModel circle:
                    return new SvgCircle()
                    {
                        CenterX = (float)circle.CenterX,
                        CenterY = (float)circle.CenterY,
                        Radius = (float)circle.Radius,
                        Stroke = circle.CurrentStrokeColor.ToSvg(),
                        StrokeWidth = (float)circle.StrokeWidth,
                        Fill = circle.Filled ? circle.FillColor.ToSvg() : SvgPaintServer.None,
                    };
            }

            return null;
        }

        public static SvgDocument ToSvg(this DocumentModel toDraw)
        {
            var r = new SvgDocument();
            for (var i = toDraw.Layers.Count - 1; i >= 0; i--)
            {
                var layer = toDraw.Layers[i];
                if (layer.IsVisible)
                    foreach (var element in layer.Shapes)
                    {
                        var tmp = element.ToSvg();
                        if (tmp != null)
                            r.Children.Add(tmp);
                    }
            }
            return r;
        }
    }
}

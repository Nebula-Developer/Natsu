using Natsu.Graphics;
using Natsu.Mathematics;

namespace Natsu.Core.Elements;

public class PolygonElement : Element {
    private Color[]? _colors;
    private bool _normalized = true;
    private int _sides = 3;
    private Vector2[]? _vertices;

    public int Sides {
        get => _sides;
        set {
            if (_sides == value) return;
            _sides = value;
            Invalidate(Invalidation.Layout);
        }
    }

    public bool Normalized {
        get => _normalized;
        set {
            if (_normalized == value) return;
            _normalized = value;
            Invalidate(Invalidation.Layout);
        }
    }

    public Color[] Colors {
        get {
            if (Invalidated.HasFlag(Invalidation.Layout)) MakeVertices();
            return _colors!;
        }
    }

    public Vector2[] Vertices {
        get {
            if (Invalidated.HasFlag(Invalidation.Layout)) MakeVertices();
            return _vertices!;
        }
    }

    protected void MakeVertices() {
        Console.WriteLine("Making vertices for shape with " + _sides + " sides");
        _vertices = new Vector2[_sides + 1];
        _colors = new Color[_sides + 1];

        float angleOffset = _sides % 2 == 0 ? MathF.PI / _sides : -MathF.PI / 2;

        for (int i = 0; i < _sides; i++) {
            float angle = MathF.PI * 2 / _sides * i + angleOffset;
            float x = MathF.Cos(angle) * (DrawSize.X * 0.5f);
            float y = MathF.Sin(angle) * (DrawSize.Y * 0.5f);

            _vertices[i] = new(x + DrawSize.X * 0.5f, y + DrawSize.Y * 0.5f);
            _colors[i] = Color.FromHSV(i / (float)_sides, 1, 1);
        }


        _vertices[_sides] = _vertices[0];
        _colors[_sides] = _colors[0];

        if (Normalized) {
            float minX = _vertices.Min(v => v.X);
            float minY = _vertices.Min(v => v.Y);
            float maxX = _vertices.Max(v => v.X);
            float maxY = _vertices.Max(v => v.Y);

            for (int i = 0; i < _vertices.Length; i++) _vertices[i] = new((_vertices[i].X - minX) / (maxX - minX) * DrawSize.X, (_vertices[i].Y - minY) / (maxY - minY) * DrawSize.Y);
        }

        Validate(Invalidation.Layout);
    }

    protected override void OnRender(ICanvas canvas) => canvas.DrawVertices(VertexMode.TriangleFan, Vertices, Colors, Paint);
}

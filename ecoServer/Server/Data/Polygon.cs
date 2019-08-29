using System.Collections.Generic;

namespace PolygonIntersection
{

    public class Polygon
    {

        private List<PolyVector> points = new List<PolyVector>();
        private List<PolyVector> edges = new List<PolyVector>();

        public void BuildEdges()
        {
            PolyVector p1;
            PolyVector p2;
            edges.Clear();
            for (int i = 0; i < points.Count; i++)
            {
                p1 = points[i];
                if (i + 1 >= points.Count)
                {
                    p2 = points[0];
                }
                else
                {
                    p2 = points[i + 1];
                }
                edges.Add(p2 - p1);
            }
        }

        public List<PolyVector> Edges {
            get { return edges; }
        }

        public List<PolyVector> Points {
            get { return points; }
            set { points = value; }
        }

        public PolyVector Center {
            get {
                float totalX = 0;
                float totalY = 0;
                for (int i = 0; i < points.Count; i++)
                {
                    totalX += points[i].X;
                    totalY += points[i].Y;
                }

                return new PolyVector(totalX / (float)points.Count, totalY / (float)points.Count);
            }
        }

        public void Offset(PolyVector v)
        {
            Offset(v.X, v.Y);
        }

        public void Offset(float x, float y)
        {
            for (int i = 0; i < points.Count; i++)
            {
                PolyVector p = points[i];
                points[i] = new PolyVector(p.X + x, p.Y + y);
            }
        }

        public override string ToString()
        {
            string result = "";

            for (int i = 0; i < points.Count; i++)
            {
                if (result != "") result += " ";
                result += "{" + points[i].ToString(true) + "}";
            }

            return result;
        }

    }

}


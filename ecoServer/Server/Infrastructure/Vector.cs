using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace PolygonIntersection {

	public struct PolyVector {

		public float X;
		public float Y;

		static public PolyVector FromPoint(Point p) {
			return PolyVector.FromPoint(p.X, p.Y);
		}

		static public PolyVector FromPoint(int x, int y) {
			return new PolyVector((float)x, (float)y);
		}

		public PolyVector(float x, float y) {
			this.X = x;
			this.Y = y;
		}

		public float Magnitude {
			get { return (float)Math.Sqrt(X * X + Y * Y); }
		}

		public void Normalize() {
			float magnitude = Magnitude;
			X = X / magnitude;
			Y = Y / magnitude;
		}

		public PolyVector GetNormalized() {
			float magnitude = Magnitude;

			return new PolyVector(X / magnitude, Y / magnitude);
		}

		public float DotProduct(PolyVector vector) {
			return this.X * vector.X + this.Y * vector.Y;
		}

		public float DistanceTo(PolyVector vector) {
			return (float)Math.Sqrt(Math.Pow(vector.X - this.X, 2) + Math.Pow(vector.Y - this.Y, 2));
		}

		public static implicit operator Point(PolyVector p) {
			return new Point((int)p.X, (int)p.Y);
		}

		public static implicit operator PointF(PolyVector p) {
			return new PointF(p.X, p.Y);
		}

		public static PolyVector operator +(PolyVector a, PolyVector b) {
			return new PolyVector(a.X + b.X, a.Y + b.Y);
		}

		public static PolyVector operator -(PolyVector a) {
			return new PolyVector(-a.X, -a.Y);
		}

		public static PolyVector operator -(PolyVector a, PolyVector b) {
			return new PolyVector(a.X - b.X, a.Y - b.Y);
		}

		public static PolyVector operator *(PolyVector a, float b) {
			return new PolyVector(a.X * b, a.Y * b);
		}

		public static PolyVector operator *(PolyVector a, int b) {
			return new PolyVector(a.X * b, a.Y * b);
		}

		public static PolyVector operator *(PolyVector a, double b) {
			return new PolyVector((float)(a.X * b), (float)(a.Y * b));
		}

		public override bool Equals(object obj) {
			PolyVector v = (PolyVector)obj;

			return X == v.X && Y == v.Y;
		}

		public bool Equals(PolyVector v) {
			return X == v.X && Y == v.Y;
		}

		public override int GetHashCode() {
			return X.GetHashCode() ^ Y.GetHashCode();
		}

		public static bool operator ==(PolyVector a, PolyVector b) {
			return a.X == b.X && a.Y == b.Y;
		}

		public static bool operator !=(PolyVector a, PolyVector b) {
			return a.X != b.X || a.Y != b.Y;
		}

		public override string ToString() {
			return X + ", " + Y;
		}

		public string ToString(bool rounded) {
			if (rounded) {
				return (int)Math.Round(X) + ", " + (int)Math.Round(Y);
			} else {
				return ToString();
			}
		}


	}

}

﻿using System;
using System.Collections.Generic;
using System.Linq;

using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;





namespace Regulus.CustomType
{

        
    public class Polygon : ICloneable , IXmlSerializable
    {
		public struct CollisionResult
		{
			public bool Intersect; // Are the polygons currently intersecting

			public Vector2 MinimumTranslationVector2; // The translation to apply to polygon A to push the polygons appart.

			public bool WillIntersect; // Are the polygons going to intersect forward in time?
		}
        
        

		private Vector2[] _Edges ;
        
        

		private Vector2[] _Points ;

	    public Polygon(Vector2[] points)
	    {
            _Edges = new Vector2[0];
            _Points = new Vector2[0];
            SetPoints(points);
        }
        public Polygon()
	    {
            _Edges = new Vector2[0];
            _Points = new Vector2[0];
        }


	    public void SetPoints( IEnumerable<Vector2> points)
	    {
	        _Points = points.ToArray();            
            _BuildEdges();
        }
        
        public Vector2[] Edges
		{
			get { return _Edges; }
		}
        
        public Vector2[] Points
		{
			get { return _Points; }
		}

		public Vector2 Center
		{
			get
			{
				float totalX = 0;
				float totalY = 0;
				for(var i = 0; i < _Points.Length; i++)
				{
					totalX += _Points[i].X;
					totalY += _Points[i].Y;
				}

				return new Vector2(totalX / _Points.Length, totalY / _Points.Length);
			}
		}

		// Structure that stores the results of the PolygonCollision function
		public static CollisionResult Collision(Polygon polygonA, Polygon polygonB, Vector2 velocity)
		{
			if(polygonA._Points.Length== 0 || polygonA._Points.Length== 0)
			{
				throw new ArgumentException("param polygonA or polygonB point count are zero.");
			}

			var result = new CollisionResult();
			result.Intersect = true;
			result.WillIntersect = true;

			var edgeCountA = polygonA.Edges.Length;
		    var edgeCountB = polygonB.Edges.Length;
			var minIntervalDistance = float.PositiveInfinity;
			var translationAxis = new Vector2();
			Vector2 edge;

			// Loop through all the edges of both polygons
			for(var edgeIndex = 0; edgeIndex < edgeCountA + edgeCountB; edgeIndex++)
			{
				if(edgeIndex < edgeCountA)
				{
					edge = polygonA.Edges[edgeIndex];
				}
				else
				{
					edge = polygonB.Edges[edgeIndex - edgeCountA];
				}

				// ===== 1. Find if the polygons are currently intersecting =====

				// Find the axis perpendicular to the current edge
				var axis = new Vector2(-edge.Y, edge.X);
				axis.Normalize();

				// Find the projection of the polygon on the current axis
				float minA = 0;
				float minB = 0;
				float maxA = 0;
				float maxB = 0;
				Polygon.ProjectPolygon(axis, polygonA, ref minA, ref maxA);
				Polygon.ProjectPolygon(axis, polygonB, ref minB, ref maxB);

				// Check if the polygon projections are currentlty intersecting
				if(Polygon.IntervalDistance(minA, maxA, minB, maxB) > 0)
				{
					result.Intersect = false;
				}

				// ===== 2. Now find if the polygons *will* intersect =====

				// Project the velocity on the current axis
				var velocityProjection = axis.DotProduct(velocity);

				// Get the projection of polygon A during the movement
				if(velocityProjection < 0)
				{
					minA += velocityProjection;
				}
				else
				{
					maxA += velocityProjection;
				}

				// Do the same test as above for the new projection
				var intervalDistance = Polygon.IntervalDistance(minA, maxA, minB, maxB);
				if(intervalDistance > 0)
				{
					result.WillIntersect = false;
				}

				// If the polygons are not intersecting and won't intersect, exit the loop
				if(!result.Intersect && !result.WillIntersect)
				{
					break;
				}

				// Check if the current interval distance is the minimum one. If so store
				// the interval distance and the current distance.
				// This will be used to calculate the minimum translation Vector2
				intervalDistance = Math.Abs(intervalDistance);
				if(intervalDistance < minIntervalDistance)
				{
					minIntervalDistance = intervalDistance;
					translationAxis = axis;

					var d = polygonA.Center - polygonB.Center;
					if(d.DotProduct(translationAxis) < 0)
					{
						translationAxis = -translationAxis;
					}
				}
			}

			// The minimum translation Vector2 can be used to push the polygons appart.
			// First moves the polygons by their velocity
			// then move polygonA by MinimumTranslationVector2.
			if(result.WillIntersect)
			{
				result.MinimumTranslationVector2 = translationAxis * minIntervalDistance;
			}

			return result;
		}

		// Calculate the distance between [minA, maxA] and [minB, maxB]
		// The distance will be negative if the intervals overlap
		public static float IntervalDistance(float minA, float maxA, float minB, float maxB)
		{
			if(minA < minB)
			{
				return minB - maxA;
			}

			return minA - maxB;
		}

		// Calculate the projection of a polygon on an axis and returns it as a [min, max] interval
		public static void ProjectPolygon(Vector2 axis, Polygon polygon, ref float min, ref float max)
		{
			// To project a point on an axis use the dot product
			var d = axis.DotProduct(polygon.Points[0]);
			min = d;
			max = d;
			for(var i = 0; i < polygon.Points.Length; i++)
			{
				d = polygon.Points[i].DotProduct(axis);
				if(d < min)
				{
					min = d;
				}
				else
				{
					if(d > max)
					{
						max = d;
					}
				}
			}
		}

		private void _BuildEdges()
		{
		    var edges = new List<Vector2>();
			Vector2 p1;
			Vector2 p2;
			
			for(var i = 0; i < _Points.Length; i++)
			{
				p1 = _Points[i];
				if(i + 1 >= _Points.Length)
				{
					p2 = _Points[0];
				}
				else
				{
					p2 = _Points[i + 1];
				}

                edges.Add(p2 - p1);
			}

		    _Edges = edges.ToArray();
		}

		public void Offset(Vector2 v)
		{
			Offset(v.X, v.Y);
		}

		public void Offset(float x, float y)
		{
			for(var i = 0; i < _Points.Length; i++)
			{
				var p = _Points[i];
				_Points[i] = new Vector2(p.X + x, p.Y + y);
			}

            _BuildEdges();
		}

		public override string ToString()
		{
			var result = string.Empty;

			for(var i = 0; i < _Points.Length; i++)
			{
				if(result != string.Empty)
				{
					result += " ";
				}

				result += "{" + _Points[i].ToString(true) + "}";
			}

			return result;
		}

	    public Polygon Clone()
	    {
            var other = new Polygon();
            other.SetPoints(_Points.ToArray());
            return other;
        }
	    object ICloneable.Clone()
	    {
	        return Clone();
	    }

	    private void MergeSort(int left, int right)
		{
			if(left < right)
			{
				var mid = (left + right) / 2;
				MergeSort(left, mid);
				MergeSort(mid + 1, right);
				Merge(left, mid, right);
			}
		}

		private void Merge(int left, int mid, int right)
		{
			int i = left, 
			    j = mid + 1, 
			    top = 0;
			var data = new Point[right + 1];
			while(i <= mid && j <= right)
			{
				if(cmp(_Points[i], _Points[j]))
				{
					data[top++] = _Points[i++];
				}
				else
				{
					data[top++] = _Points[j++];
				}
			}

			while(i <= mid)
			{
				data[top++] = _Points[i++];
			}

			while(j <= right)
			{
				data[top++] = _Points[j++];
			}

			for(i = 0, j = left; i < top; i++, j++)
			{
				_Points[j] = Vector2.FromPoint(data[i].X, data[i].Y);
			}
		}

		private float cross(Point o, Point a, Point b)
		{
			return (a.X - o.X) * (b.Y - o.Y) - (a.Y - o.Y) * (b.X - o.X);
		}

		private bool cmp(Point a, Point b)
		{
			return (a.Y < b.Y) || (a.Y == b.Y && a.X < b.X);
		}

		public void Convex()
		{
			MergeSort(0, _Points.Length- 1);

			var CH = new Vector2[_Points.Length+ 1];
			var m = 0;
			for(var i = 0; i < _Points.Length; i++)
			{
				while(m >= 2 && cross(CH[m - 2], CH[m - 1], _Points[i]) <= 0)
				{
					m--;
				}

				CH[m++] = _Points[i];
			}

			for(int i = _Points.Length- 2, 
			        t = m + 1;
			    i >= 0;
			    i--)
			{
				while(m >= t && cross(CH[m - 2], CH[m - 1], _Points[i]) <= 0)
				{
					m--;
				}

				CH[m++] = _Points[i];
			}

            SetPoints(CH);
		}


        public void RotationByDegree(float degree)
        {
            Rotation(degree * (float)Math.PI / 180f , new Vector2(Center.X , Center.Y));
        }

        public void RotationByDegree(float degree , Vector2 center)
        {
            Rotation(degree * (float)Math.PI / 180f , center);
        }

        public void Rotation(float radians , Vector2 center)
        {
            var points = new List<Vector2>();            
            foreach (var point in Points)
            {
                points.Add(Polygon.RotatePoint(point, center, radians));
            }

            SetPoints(points);
        }
        public void Rotation(float radians)
        {
            var center = new Vector2(Center.X, Center.Y);
            Rotation(radians , center);            
		}

		public  static Vector2 RotatePoint(Vector2 point, Vector2 centroid, double angle)
		{
			var x = centroid.X + ((point.X - centroid.X) * Math.Cos(angle) - (point.Y - centroid.Y) * Math.Sin(angle));

			var y = centroid.Y + ((point.X - centroid.X) * Math.Sin(angle) + (point.Y - centroid.Y) * Math.Cos(angle));

			return new Vector2((float)x, (float)y);
		}

        XmlSchema IXmlSerializable.GetSchema()
        {
            throw new NotImplementedException();
        }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            
            var sub = reader.ReadSubtree();
            if (sub.ReadToFollowing("ArrayOfVector2"))
            {
                var serializer = new XmlSerializer(typeof(Vector2[]));
                var points = (Vector2[])serializer.Deserialize(sub);

                
                sub.Close();

                SetPoints(points);

            }
            reader.Skip();
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            new XmlSerializer(typeof(Vector2[])).Serialize(writer, _Points, ns);
        }
    }
}

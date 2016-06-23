using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;
using OutlineWF.Utilities;

namespace OutlineWF.Models
{
    public class Detail
    {
        private string _name;
        private string _details;
        private List<Part> _parts;

        public Detail()
        {
            _name = "Detal";
            _details = "aaaa";
            _parts = new List<Part>();
        }

        public void AddPart(Part part)
        {
            _parts.Add(part);
        }

        public Part LastPart
        {
            get { return _parts.Last(); }
        }

        public List<Part> Parts
        {
            get { return _parts; }
        }

        public string ToDXT()
        {
            return null;
        }

        public string ToDGT()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(_name);
            sb.AppendLine(_details);
            sb.AppendLine(_parts.Count.ToString());
            for (int i = 0; i < _parts.Count; i++)
            {
                sb.Append("Detal").AppendLine(i.ToString());

            }
            for (int i = 0; i < _parts.Count; i++)
            {
                sb.AppendLine(_parts[i].Vertices.Count.ToString());
            }
            for (int i = 0; i < _parts.Count; i++)
            {
                if (_parts[i].Vertices.Count == 0) continue;

                sb.Append(_parts[i].Vertices[0]).Append("   ").Append("Detal").AppendLine(i.ToString());
                for (int j = 1; j < _parts[i].Vertices.Count; j++)
                {
                    sb.AppendLine(_parts[i].Vertices[j].ToString());
                }
            }
            return sb.ToString();
        }

        public static Detail ReadDGT(StreamReader reader)
        {
            var res = new Detail();

            res._name = reader.ReadLine();
            res._details = reader.ReadLine();
            res._parts = new List<Part>(Convert.ToInt32(reader.ReadLine()));
            for (int i = 0; i < res._parts.Capacity; i++)
            {
                res._parts.Add(new Part(reader.ReadLine()));
            }
            for (int i = 0; i < res._parts.Count; i++)
            {
                res._parts[i].Vertices = new List<Point>(Convert.ToInt32(reader.ReadLine()));
            }

            int partIndex = 0;
            int pointIndex = -1;
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                if (line != null)
                {
                    var coords = Regex.Split(line, "\\s+").Where(s => !string.Empty.Equals(s)).Take(2).
                        Select(s => Convert.ToSingle(s, CultureInfo.InvariantCulture)).ToList();
                    if (pointIndex == res._parts[partIndex].Vertices.Capacity - 1)
                    {
                        partIndex++;
                        pointIndex = -1;
                    }
                    res._parts[partIndex].AddPoint(new Point(coords[0], coords[1]));
                    pointIndex++;
                }
                else
                {
                    throw new FileLoadException("Unexpected empty line.");
                }
            }
            return res;
        }

        public string ToDXF()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("0");
            sb.AppendLine("SECTION").AppendLine("2");
            sb.AppendLine("ENTITIES").AppendLine("0");

            int index = -1;
            foreach (var part in _parts)
            {
                index++;
                for (int i = 0; i < part.Vertices.Count - 1; i++)
                {
                    var start = part.Vertices[i];
                    var end = part.Vertices[i + 1];
                    sb.AppendLine("LINE").AppendLine(Convert.ToString(8)).AppendLine(Convert.ToString(index));

                    sb.AppendLine("10").AppendLine(Convert.ToString(start.X));
                    sb.AppendLine("20").AppendLine(Convert.ToString(start.Y));
                    sb.AppendLine("30").AppendLine("0.0");

                    sb.AppendLine("11").AppendLine(Convert.ToString(end.X));
                    sb.AppendLine("21").AppendLine(Convert.ToString(end.Y));
                    sb.AppendLine("31").AppendLine("0.0");
                    sb.AppendLine("0");
                }
                var start1 = part.Vertices[part.Vertices.Count - 1];
                var end1 = part.Vertices[0];
                sb.AppendLine("LINE").AppendLine(Convert.ToString(8)).AppendLine(Convert.ToString(0));

                sb.AppendLine("10").AppendLine(Convert.ToString(start1.X));
                sb.AppendLine("20").AppendLine(Convert.ToString(start1.Y));
                sb.AppendLine("30").AppendLine("0.0");

                sb.AppendLine("11").AppendLine(Convert.ToString(end1.X));
                sb.AppendLine("21").AppendLine(Convert.ToString(end1.Y));
                sb.AppendLine("31").AppendLine("0.0");
                sb.AppendLine("0");
            }
            sb.AppendLine("ENDSEC").AppendLine("0").AppendLine("EOF");
            return sb.ToString();
        }

        public static Detail ReadDXF(StreamReader reader)
        {
            var res = new Detail();
            for (int i = 0; i < 5; i++)
                reader.ReadLine();
            var lastLayerId = -1;
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (line.Equals("ENDSEC")) break;
                reader.ReadLine();
                var layerId = Convert.ToInt32(reader.ReadLine());
                if (layerId != lastLayerId)
                {
                    lastLayerId++;
                    res.AddPart(new Part());
                }
                reader.ReadLine();
                var startX = Convert.ToSingle(reader.ReadLine());
                reader.ReadLine();
                var startY = Convert.ToSingle(reader.ReadLine());
                reader.ReadLine();
                reader.ReadLine();
                reader.ReadLine();
                var endX = Convert.ToSingle(reader.ReadLine());
                reader.ReadLine();
                var endY = Convert.ToSingle(reader.ReadLine());
                reader.ReadLine();
                reader.ReadLine();
                reader.ReadLine();
                res._parts[lastLayerId].AddPoint(new Point(startX, startY));
                res._parts[lastLayerId].AddPoint(new Point(endX, endY));
            }
            return res;
        }
    }
}

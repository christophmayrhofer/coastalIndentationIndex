using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System;

using ESRI.ArcGIS.ArcMap;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.CartoUI;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.DisplayUI;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.GeoDatabaseUI;
using ESRI.ArcGIS.Editor;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Framework;


namespace IndentationIndex
{
    public partial class IIGUI : Form
    {
        IFeatureClass pCls = ((IGeoFeatureLayer)ArcMap.Document.FocusMap.get_Layer(0)).FeatureClass;
        ICurve pCurve = ((IGeometry)((IPolyline)((IGeoFeatureLayer)ArcMap.Document.FocusMap.get_Layer(0)).FeatureClass.GetFeature(0).Shape)) as ICurve;
        IUnitConverter conv = new UnitConverterClass();
        esriUnits unit;
        esriUnits mapunit = ArcMap.Document.FocusMap.MapUnits;

        public IIGUI()
        {
            InitializeComponent();
            if(conv.ConvertUnits(pCurve.Length,mapunit,esriUnits.esriKilometers) < 100)
                combox_unit.SelectedIndex = 0;
            else 
                combox_unit.SelectedIndex = 1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveFileDialog savedia = new SaveFileDialog();
            savedia.Filter = ".csv|*.csv";
            savedia.ShowDialog();
            if (savedia.FileName == "")
            {
                MessageBox.Show("Invalid Filename");
                return;
            }
            StreamWriter sw = new StreamWriter(savedia.FileName);
            II(sw);
            MessageBox.Show("File saved to: " + savedia.FileName);
        }

        public void AddGraphicToMap(IActiveView view, IGeometry geometry, IRgbColor rgbColor, IRgbColor outlineRgbColor, int width)
        {
            IGraphicsContainer graphicsContainer = (IGraphicsContainer)view; // Explicit Cast
            IElement element = null;
            if ((geometry.GeometryType) == esriGeometryType.esriGeometryPoint)
            {
                // Marker symbols
                ISimpleMarkerSymbol simpleMarkerSymbol = new SimpleMarkerSymbolClass();
                simpleMarkerSymbol.Color = rgbColor;
                simpleMarkerSymbol.Outline = true;
                simpleMarkerSymbol.OutlineColor = outlineRgbColor;
                simpleMarkerSymbol.Size = width * 3;
                simpleMarkerSymbol.Style = ESRI.ArcGIS.Display.esriSimpleMarkerStyle.esriSMSCircle;

                IMarkerElement markerElement = new MarkerElementClass();
                markerElement.Symbol = simpleMarkerSymbol;
                element = (IElement)markerElement; // Explicit Cast
            }
            else if ((geometry.GeometryType) == ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolyline ||
                     (geometry.GeometryType) == ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryCircularArc)
            {
                //  Line elements
                ISimpleLineSymbol simpleLineSymbol = new SimpleLineSymbolClass();
                simpleLineSymbol.Color = rgbColor;
                simpleLineSymbol.Style = esriSimpleLineStyle.esriSLSSolid;
                simpleLineSymbol.Width = width;

                ILineElement lineElement = new LineElementClass();
                lineElement.Symbol = simpleLineSymbol;
                element = (IElement)lineElement; // Explicit Cast
            }
            else if ((geometry.GeometryType) == ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon)
            {
                // Polygon elements
                ESRI.ArcGIS.Display.ISimpleFillSymbol simpleFillSymbol = new ESRI.ArcGIS.Display.SimpleFillSymbolClass();
                simpleFillSymbol.Color = rgbColor;
                simpleFillSymbol.Style = ESRI.ArcGIS.Display.esriSimpleFillStyle.esriSFSForwardDiagonal;
                ESRI.ArcGIS.Carto.IFillShapeElement fillShapeElement = new ESRI.ArcGIS.Carto.PolygonElementClass();
                fillShapeElement.Symbol = simpleFillSymbol;
                element = (ESRI.ArcGIS.Carto.IElement)fillShapeElement; // Explicit Cast
            }
            if (!(element == null))
            {
                element.Geometry = geometry;
                graphicsContainer.AddElement(element, 0);
                view.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
            }
        }

        private double GetDistAlong(IPolyline polyLine, IPoint pnt)
        {
            var outPnt = new PointClass() as IPoint;
            double distAlong = double.NaN;
            double distFrom = double.NaN;
            bool bRight = false;
            polyLine.QueryPointAndDistance(esriSegmentExtension.esriNoExtension, pnt, false, outPnt,
                ref distAlong, ref distFrom, ref bRight);
            return distAlong;
        }

        private void trkbar_seg_ValueChanged(object sender, EventArgs e)
        {
            txtbox_seg.Text = trkbar_seg.Value.ToString();
            II(null);

        }

        private void II(StreamWriter sw)
        {
            if (sw != null)
                sw.WriteLine("Distance;Eucl.Dist.;II");

            ArcMap.Document.ActiveView.GraphicsContainer.DeleteAllElements();

            IRgbColor red = new RgbColor();
            red.Red = 255;

            IRgbColor blue = new RgbColor();
            blue.Blue = 255;

            IRgbColor green = new RgbColor();
            green.Green = 255;

            double segin = trkbar_seg.Value;
            double seg = conv.ConvertUnits(segin, unit, mapunit);
            List<IPoint> pPoints = new List<IPoint>();
            pPoints.Add(pCurve.FromPoint);

            double disttot = 0;
            double eucltot = 0;

            while (true)
            {
                IPoint pPointSectPrev = pPoints.ElementAt(pPoints.Count - 1);

                ICircularArc circularArc = new CircularArcClass();
                ((IConstructCircularArc2)circularArc).ConstructCircle(pPointSectPrev, seg, true);
                ISegment Seg = (ISegment)circularArc;
                ISegmentCollection SegCollection = new PolylineClass() as ISegmentCollection;
                object Missing = System.Type.Missing;
                SegCollection.AddSegment(Seg, ref Missing, ref Missing);
                IGeometry pCircle = SegCollection as IGeometry;

                if (chkbox_con.Checked)
                    AddGraphicToMap(ArcMap.Document.ActiveView, pCircle, blue, blue, 1);

                IPointCollection pSectPoints = ((ITopologicalOperator)pCurve).Intersect(pCircle, esriGeometryDimension.esriGeometry0Dimension) as IPointCollection;
                IPoint pPointSect = null;

                for (int j = 0; j < pSectPoints.PointCount; j++)
                {
                    if (GetDistAlong(pCurve as IPolyline, pSectPoints.get_Point(j)) >
                       GetDistAlong(pCurve as IPolyline, pPointSectPrev))
                    {
                        if (pPointSect == null)
                            pPointSect = pSectPoints.get_Point(j);
                        else if (GetDistAlong(pCurve as IPolyline, pSectPoints.get_Point(j)) <
                                 GetDistAlong(pCurve as IPolyline, pPointSect))
                            pPointSect = pSectPoints.get_Point(j);
                    }
                }
                if (pPointSect == null)
                    pPointSect = pCurve.ToPoint;

                pPoints.Add(pPointSect);
                if (chkbox_con.Checked)
                    AddGraphicToMap(ArcMap.Document.ActiveView, pPointSect as IGeometry, blue, blue, 1);

                double d1 = GetDistAlong(pCurve as IPolyline, pPointSectPrev);
                double d2 = GetDistAlong(pCurve as IPolyline, pPointSect);

                ICurve pSubCurve;
                pCurve.GetSubcurve(d1, d2, false, out pSubCurve);

                double dist = conv.ConvertUnits(pSubCurve.Length, ArcMap.Document.FocusMap.MapUnits, esriUnits.esriMeters);
                double eucl = conv.ConvertUnits(((IProximityOperator)pSubCurve.FromPoint).ReturnDistance(pSubCurve.ToPoint), ArcMap.Document.FocusMap.MapUnits, esriUnits.esriMeters);
                double II = dist / eucl;

                disttot += dist;
                eucltot += eucl;

                if (sw != null)
                    sw.WriteLine(disttot.ToString("0") + ";" + eucltot.ToString("0") + ";" + II.ToString("0.00"));

                IRgbColor pCol = new RgbColorClass();
                if (II < 1.1)
                    pCol.Green = 255;
                else if (II < 1.25)
                {
                    pCol.Red = 192;
                    pCol.Green = 255;
                }
                else if (II < 1.5)
                {
                    pCol.Red = 255;
                    pCol.Green = 255;
                }
                else if (II < 1.75)
                {
                    pCol.Red = 255;
                    pCol.Green = 192;
                }
                else
                    pCol.Red = 255;

                if (chkbox_cat.Checked)
                    AddGraphicToMap(ArcMap.Document.ActiveView, pSubCurve as IGeometry, pCol, pCol, 2);

                IPolyline pSubLine = new PolylineClass();
                pSubLine.FromPoint = pPointSectPrev;
                pSubLine.ToPoint = pPointSect;
                if (chkbox_con.Checked)
                    AddGraphicToMap(ArcMap.Document.ActiveView, pSubLine as IGeometry, blue, blue, 1);

                if (GetDistAlong(pCurve as IPolyline, pPointSect) == pCurve.Length)
                {
                    if (sw != null)
                        sw.Close();
                    break;
                }
            }
        }

        private void II_analyze(StreamWriter sw)
        {
            if (sw == null)
                return;

            List<List<double>> pLevelsDist = new List<List<double>>();
            List<List<double>> pLevelsEucl = new List<List<double>>();
            List<List<double>> pLevelsII = new List<List<double>>();

            bool lastlevel = false;
            while (!lastlevel)
            {
                double segin = trkbar_seg.Value * Math.Pow(2, (pLevelsII.Count));
                double seg = conv.ConvertUnits(segin, unit, mapunit);
                List<IPoint> pPoints = new List<IPoint>();
                pPoints.Add(pCurve.FromPoint);

                List<double> pDist = new List<double>();
                List<double> pEucl = new List<double>();
                List<double> pII = new List<double>();

                bool lastpoint = false;
                while (!lastpoint)
                {
                    System.Windows.Forms.Application.DoEvents();
                    IPoint pPointSectPrev = pPoints.ElementAt(pPoints.Count - 1);

                    ICircularArc circularArc = new CircularArcClass();
                    ((IConstructCircularArc2)circularArc).ConstructCircle(pPointSectPrev, seg, true);
                    ISegment Seg = (ISegment)circularArc;
                    ISegmentCollection SegCollection = new PolylineClass() as ISegmentCollection;
                    object Missing = System.Type.Missing;
                    SegCollection.AddSegment(Seg, ref Missing, ref Missing);
                    IGeometry pCircle = SegCollection as IGeometry;

                    IPointCollection pSectPoints = ((ITopologicalOperator)pCurve).Intersect(pCircle, esriGeometryDimension.esriGeometry0Dimension) as IPointCollection;
                    if (pSectPoints.PointCount == 0)
                    {
                        lastlevel = true;
                        break;
                    }
                    IPoint pPointSect = null;
                    for (int j = 0; j < pSectPoints.PointCount; j++)
                    {
                        if (GetDistAlong(pCurve as IPolyline, pSectPoints.get_Point(j)) >
                           GetDistAlong(pCurve as IPolyline, pPointSectPrev))
                        {
                            if (pPointSect == null)
                                pPointSect = pSectPoints.get_Point(j);
                            else if (GetDistAlong(pCurve as IPolyline, pSectPoints.get_Point(j)) <
                                     GetDistAlong(pCurve as IPolyline, pPointSect))
                                pPointSect = pSectPoints.get_Point(j);
                        }
                    }
                    if (pPointSect == null)
                    {
                        pPointSect = pCurve.ToPoint;
                        lastpoint = true;
                    }

                    pPoints.Add(pPointSect);

                    double d1 = GetDistAlong(pCurve as IPolyline, pPointSectPrev);
                    double d2 = GetDistAlong(pCurve as IPolyline, pPointSect);
                    ICurve pSubCurve;
                    pCurve.GetSubcurve(d1, d2, false, out pSubCurve);

                    double dist = conv.ConvertUnits(pSubCurve.Length, ArcMap.Document.FocusMap.MapUnits, esriUnits.esriMeters);
                    double eucl = conv.ConvertUnits(((IProximityOperator)pSubCurve.FromPoint).ReturnDistance(pSubCurve.ToPoint), ArcMap.Document.FocusMap.MapUnits, esriUnits.esriMeters);
                    double II = dist / eucl;

                    pDist.Add(dist);
                    pEucl.Add(eucl);
                    pII.Add(II);
                }

                if (pII.Count == 0)
                    break;

                pLevelsII.Add(pII);
                pLevelsEucl.Add(pEucl);
                pLevelsDist.Add(pDist);
            }

            int levels = pLevelsII.Count;
            sw.Write("Segment");
            for (int i = 1; i <= levels; i++)
            {
                double eucl = conv.ConvertUnits(pLevelsEucl.ElementAt(i - 1).First(), mapunit, unit);
                double eucllast = conv.ConvertUnits(pLevelsEucl.ElementAt(i - 1).Last(), mapunit, unit);
                sw.Write(";" + i.ToString() + "(" + (pLevelsII.ElementAt(i - 1).Count - 1) + " x " + eucl.ToString("0") + " + 1 x " + eucllast.ToString("0") + " " + combox_unit.Text + ")");
            }
            sw.WriteLine();

            for (int i = 0; i < pLevelsII.ElementAt(0).Count; i++)
            {
                string line = (i + 1).ToString();
                for (int j = 0; j < levels; j++)
                {
                    System.Windows.Forms.Application.DoEvents();
                    try
                    {
                        line = line + ";" + pLevelsII.ElementAt(j).ElementAt(i).ToString("0.00");
                    }
                    catch
                    {
                        line = line + ";";
                    }
                }
                sw.WriteLine(line);
            }
            sw.Close();

        }
       
        private void txtbox_max_TextChanged(object sender, EventArgs e)
        {
            trkbar_seg.Maximum = Convert.ToInt32(txtbox_max.Text);
            trkbar_seg.SmallChange = trkbar_seg.Maximum / 50;
            trkbar_seg.TickFrequency = trkbar_seg.Maximum / 5;
            trkbar_seg.LargeChange = trkbar_seg.Maximum / 5;
        }

        private void txtbox_min_TextChanged(object sender, EventArgs e)
        {
            trkbar_seg.Minimum = Convert.ToInt32(txtbox_min.Text);
        }

        private void combox_unit_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (combox_unit.SelectedIndex)
            {
                case 0:
                    unit = esriUnits.esriMeters;
                    break;
                case 1:
                    unit = esriUnits.esriKilometers;
                    break;
                case 2:
                    unit = esriUnits.esriFeet;
                    break;
                case 3:
                    unit = esriUnits.esriMiles;
                    break;
                default:
                    unit = esriUnits.esriMeters;
                    break;
            }

            double len = conv.ConvertUnits(pCurve.Length, mapunit, unit);
            trkbar_seg.Maximum = (int)len / 3;
            trkbar_seg.Minimum = (int)len / 300;
            trkbar_seg.Value = (int)len / 30;
            txtbox_seg.Text = ((int)len / 30).ToString();
            txtbox_min.Text = ((int)len / 300).ToString();
            txtbox_max.Text = ((int)len / 3).ToString();
        }

        private void chkbox_cat_CheckedChanged(object sender, EventArgs e)
        {
            II(null);
        }

        private void chkbox_con_CheckedChanged(object sender, EventArgs e)
        {
            II(null);
        }

        private void btn_analyze_Click(object sender, EventArgs e)
        {
            SaveFileDialog savedia = new SaveFileDialog();
            savedia.Filter = ".csv|*.csv";
            savedia.ShowDialog();
            if (savedia.FileName == "")
            {
                MessageBox.Show("Invalid Filename");
                return;
            }
            StreamWriter sw = new StreamWriter(savedia.FileName);
            II_analyze(sw);
            MessageBox.Show("File saved to: " + savedia.FileName);
        }

        private void txtbox_seg_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (Convert.ToInt32(txtbox_seg.Text) < trkbar_seg.Minimum)
                {
                    trkbar_seg.Minimum = Convert.ToInt32(txtbox_seg.Text);
                    txtbox_min.Text = trkbar_seg.Minimum.ToString();
                }
                if (Convert.ToInt32(txtbox_seg.Text) > trkbar_seg.Maximum)
                {
                    trkbar_seg.Minimum = Convert.ToInt32(txtbox_seg.Text);
                    txtbox_max.Text = trkbar_seg.Maximum.ToString();
                }
                trkbar_seg.Value = Convert.ToInt32(txtbox_seg.Text);
            }
        }

    }
}

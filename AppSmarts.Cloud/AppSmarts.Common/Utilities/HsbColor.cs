using System;
using System.Drawing;

namespace AppSmarts.Common.Utilities
{
    public struct HsbColor
    {
        double _h;
        double _s;
        double _b;
        int _a;

        public HsbColor(double h, double s, double b)
        {
            _a = 0xff;
            _h = Math.Min(Math.Max(h, 0), 255);
            _s = Math.Min(Math.Max(s, 0), 255);
            _b = Math.Min(Math.Max(b, 0), 255);
        }

        public HsbColor(int a, double h, double s, double b)
        {
            _a = a;
            _h = Math.Min(Math.Max(h, 0), 255);
            _s = Math.Min(Math.Max(s, 0), 255);
            _b = Math.Min(Math.Max(b, 0), 255);
        }

        public HsbColor(Color color)
        {
            HsbColor temp = FromColor(color);
            _a = temp._a;
            _h = temp._h;
            _s = temp._s;
            _b = temp._b;
        }

        public double H
        {
            get { return _h; }
        }

        public double S
        {
            get { return _s; }
        }

        public double B
        {
            get { return _b; }
        }

        public int A
        {
            get { return _a; }
        }

        public Color Color
        {
            get
            {
                return FromHsb(this);
            }
        }

        public static Color ShiftHue(Color c, double hueDelta)
        {
            HsbColor hsb = FromColor(c);
            hsb._h += hueDelta;
            hsb._h = Math.Min(Math.Max(hsb._h, 0), 255);
            return FromHsb(hsb);
        }

        public static Color ShiftSaturation(Color c, double saturationDelta)
        {
            HsbColor hsb = FromColor(c);
            hsb._s += saturationDelta;
            hsb._s = Math.Min(Math.Max(hsb._s, 0), 255);
            return FromHsb(hsb);
        }


        public static Color ShiftBrighness(Color c, double brightnessDelta)
        {
            HsbColor hsb = FromColor(c);
            hsb._b += brightnessDelta;
            hsb._b = Math.Min(Math.Max(hsb._b, 0), 255);
            return FromHsb(hsb);
        }

        public static Color FromHsb(HsbColor hsbColor)
        {
            double r = hsbColor._b;
            double g = hsbColor._b;
            double b = hsbColor._b;
            if (Math.Abs(hsbColor._s - 0) > double.Epsilon)
            {
                double max = hsbColor._b;
                double dif = hsbColor._b * hsbColor._s / 255f;
                double min = hsbColor._b - dif;

                double h = hsbColor._h * 360f / 255f;

                if (h < 60f)
                {
                    r = max;
                    g = h * dif / 60f + min;
                    b = min;
                }
                else if (h < 120f)
                {
                    r = -(h - 120f) * dif / 60f + min;
                    g = max;
                    b = min;
                }
                else if (h < 180f)
                {
                    r = min;
                    g = max;
                    b = (h - 120f) * dif / 60f + min;
                }
                else if (h < 240f)
                {
                    r = min;
                    g = -(h - 240f) * dif / 60f + min;
                    b = max;
                }
                else if (h < 300f)
                {
                    r = (h - 240f) * dif / 60f + min;
                    g = min;
                    b = max;
                }
                else if (h <= 360f)
                {
                    r = max;
                    g = min;
                    b = -(h - 360f) * dif / 60 + min;
                }
                else
                {
                    r = 0;
                    g = 0;
                    b = 0;
                }
            }

            return Color.FromArgb
                (
                    hsbColor._a,
                    (int)Math.Round(Math.Min(Math.Max(r, 0), 255)),
                    (int)Math.Round(Math.Min(Math.Max(g, 0), 255)),
                    (int)Math.Round(Math.Min(Math.Max(b, 0), 255))
                    );
        }

        public static HsbColor FromColor(Color color)
        {
            HsbColor ret = new HsbColor(0f, 0f, 0f) {_a = color.A};

            double r = color.R;
            double g = color.G;
            double b = color.B;

            double max = Math.Max(r, Math.Max(g, b));

            if (max <= 0)
            {
                return ret;
            }

            double min = Math.Min(r, Math.Min(g, b));
            double dif = max - min;

            if (max > min)
            {
                if (Math.Abs(g - max) < double.Epsilon)
                {
                    ret._h = (b - r) / dif * 60f + 120f;
                }
                else if (Math.Abs(b - max) < double.Epsilon)
                {
                    ret._h = (r - g) / dif * 60f + 240f;
                }
                else if (b > g)
                {
                    ret._h = (g - b) / dif * 60f + 360f;
                }
                else
                {
                    ret._h = (g - b) / dif * 60f;
                }
                if (ret._h < 0)
                {
                    ret._h = ret._h + 360f;
                }
            }
            else
            {
                ret._h = 0;
            }

            ret._h *= 255f / 360f;
            ret._s = (dif / max) * 255f;
            ret._b = max;

            return ret;
        }
    }
}

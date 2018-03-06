using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class PlatformVMB : ViewModelBase
    {
        double _x;
        double _y;

        public PlatformVMB(double x, double y)
        {
            Width = 200;
            X = x;
            Y = y;

        }
       
        public double Width { get; set; }

        public double X
        {
            get => _x;
            set
            {
                if (_x == value) return;
                _x = value;
                FirePropertyChanged();
            }
        }

        public double Y
        {
            get => _y;
            set
            {
                if (_y == value) return;
                _y = value;
                FirePropertyChanged();
            }
        }

        public double[] getYXleftXright()
        {
            var tempArray = new double[3];
            tempArray[0] = Y;
            tempArray[1] = X;
            tempArray[2] = X + Width;
            return tempArray;
        }
    }
}

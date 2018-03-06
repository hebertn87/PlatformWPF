using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Media;
using System.Collections.ObjectModel;

namespace WpfApp1
{
    public class MajorTomVMB : ViewModelBase
    { 
        //Initialization of major tom boiii
        public MajorTomVMB(double x, double y, ObservableCollection<PlatformVMB> _Platforms)
        {
            X = x;
            Y = y;
            verticalVelocity = _vertical = 0;
            horizontalVelocity = _horizontal = 0;
            Size = 50;
            
            Platforms = _Platforms;
        }

        //**********************************************************************************************\\
        //                                          MEMBERS                                             \\
        //______________________________________________________________________________________________\\

        double _x;
        double _y;
        double _vertical;
        double _horizontal;
        double maxVelocityRight = 1;
        double maxVelocityLeft = -1;
        bool isGrounded = false;
        ObservableCollection<PlatformVMB> Platforms;
        
        

        public double Size { get; set; }

        //**********************************************************************************************\\
        //                                    STATUS/PROPERTIES                                         \\
        //______________________________________________________________________________________________\\


        //POSITION PROPERTIES
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

        public void movePlat()
        {
            foreach (var platty in Platforms)
            {
                platty.Y += .1;
            }
        }

        //PHYSICAL STATUSES
        bool isRunning = false;
        public bool IsRunning { get => isRunning; set => isRunning = value; }

        public double verticalVelocity
        {
            get => _vertical;
            set
            {
                if (_vertical == value) return;
                _vertical = value;
                FirePropertyChanged();
            }
        }

        public double horizontalVelocity
        {
            get => _horizontal;
            set
            {
                if (_horizontal == value) return;
                _horizontal = value;
               FirePropertyChanged();
            }
        }

        //**********************************************************************************************\\
        //                                      FUNCTIONS                                               \\
        //______________________________________________________________________________________________\\

        //Movement Related functions
        public void MoveRight()
        {
            if(horizontalVelocity <= maxVelocityRight)
            {
                if (Grounded())
                    horizontalVelocity += .1;
                else
                    horizontalVelocity += .01;
            }
        }

        public void MoveLeft()
        {
            if (horizontalVelocity >= maxVelocityLeft)
            {
                if (Grounded())
                    horizontalVelocity -= .1;
                else
                    horizontalVelocity -= .01;
            }
        }

        public void Jump()
        {
           if (isGrounded)
           {
                Y -= 40;
                verticalVelocity = -1;
                isGrounded = false;
           }
        }

        //Status related functions
        public bool Grounded()
        {
            return isGrounded;
        }

        //public bool willCollideVert()
        //{
            //foreach platform 
            //if toms y plus his height will hit or go past platform y and tom is in the same column as the platform but not below it
            //set tom Y + height to platform y
            //tom vert velocity = 0
            //return true
            //else return false
        //}

        public double[] whereTomFeet()
        {
            var tempArray = new double[3];
            tempArray[0] = Y - Size;
            tempArray[1] = X;
            tempArray[2] = X + Size;

            return tempArray;
        }

        //_______________________________________________________
        //                      UPDATE                          -
        //‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾
        //Collision functions
        public bool willCollideVertical()
        {
            foreach (var platty in Platforms)
            {
                var platformYX1X2 = platty.getYXleftXright();
                var tomYX1X2 = whereTomFeet();
                if (//if tom will hit platform next tick
                    Y + Size + verticalVelocity >= platformYX1X2[0]
                    //and if tom's feet are within the bounds of the platform
                    && ((tomYX1X2[1] > platformYX1X2[1] && tomYX1X2[1] < platformYX1X2[2]) || (tomYX1X2[2] > platformYX1X2[1] && tomYX1X2[2] < platformYX1X2[2]))
                    //and if tom is above the platform(not below)
                    && Y < platformYX1X2[0])
                {
                    verticalVelocity = 0;
                    Y = platformYX1X2[0] - Size;
                    isGrounded = true;
                    return true;
                }
                else
                {
                    isGrounded = false;
      
                }
            }
            
            return false;
        }

        public bool willCollideHorizontal()
        {
            return false;
        }
    }
}

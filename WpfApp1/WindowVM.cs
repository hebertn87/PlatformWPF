using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using NAudio.Wave;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;

namespace WpfApp1
{
    public class WindowVM : ViewModelBase
    {

        Dictionary<Key, bool> keyDown= new Dictionary<Key, bool>();


        //**********************************************************************************************\\
        //                                        Initialization                                        \\
        //______________________________________________________________________________________________\\
        public WindowVM()
        {
            //setup sound here UPDATE

            //GAMEOBJECT INITIALIZATION
            MajorToms = new ObservableCollection<MajorTomVMB>();
            Platforms = new ObservableCollection<PlatformVMB>();
            Platform = new ObservableCollection<PlatformVMB>();
            Platform.Add(new PlatformVMB(0, 0));
            //PLACEMENT
            var rand = new Random();
            Platforms.Add(new PlatformVMB(0, 728));
            Platforms.Add(new PlatformVMB(300, 700));
            Platforms.Add(new PlatformVMB(600, 670));
            Platforms.Add(new PlatformVMB(900, 640));
            Platforms.Add(new PlatformVMB(1200, 610));
            Platforms.Add(new PlatformVMB(1400, 730));

            MajorToms.Add(new MajorTomVMB(100, 0, Platforms));
            //COMMANDS6
            jump = new testCommand(O => MajorToms[0].Jump());

            //UTILITY INITIALIZATION
            foreach (var key in Enum.GetValues(typeof(Key)))
            {
                keyDown[(Key)key] = false;
            }

            //START THREAD
            var thread = new Thread(UpdateThread)
            {
                IsBackground = true
            };

            thread.Start();
        }

        //**********************************************************************************************\\
        //                                   KEYPRESS DETECTION                                         \\
        //______________________________________________________________________________________________\\

        public void KeyDown(Key k)
        {
            keyDown[k] = true;
        }

        public void KeyUp(Key k)
        {
            keyDown[k] = false;
            MajorToms[0].IsRunning = false;
        }

        //**********************************************************************************************\\
        //                                        GAME LOOP                                             \\
        //______________________________________________________________________________________________\\

        private void UpdateThread()
        {
            while (true)
            {
                //GET INPUTS
                var Tom = MajorToms[0];

                if (keyDown[Key.A])
                {
                    Tom.MoveLeft();
                }
                if (keyDown[Key.Space])
                {
                    Tom.Jump();
                }
                if(keyDown[Key.D])
                {
                    Tom.MoveRight();
                }


                //_______________________________________________________
                //                      UPDATE ME                       -
                //‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾
                
                //IF YOU SEE Y > 400 REPLACE THIS WITH Tom.isGrounded once implemented

                //Gravity
                //IF TOM IS NOT GOING TO COLLIDE WITH ANYTHING VERTICALLY
                //      INCREASE VERTICAL VELOCITY
                //Friction
                //IF NOT GOING TO COLLIDE WITH ANYTHING HORIZONTALLY AND IS NOT RUNNING
                //      DECREASE HORIZONTAL VELOCITY
                //ELSE IF
                //      GOING TO COLLIDE VERTICAL verticalVelocity = 0
                //ELSE IF
                //      GOING TO COLLIDE HORIZONTALLY horizontalVelocity = 0
                
                //Gravity
                if(!Tom.willCollideVertical())
                {
                    Tom.verticalVelocity += .009;
                }
                //Friction
                //if tom is running and is not going to hit anything let him continue
                if (Tom.IsRunning && !Tom.willCollideHorizontal())
                {
                    //keep running
                }
                //if tom is running but will hit something stop him
                if (Tom.IsRunning && Tom.willCollideHorizontal())
                {
                    Tom.horizontalVelocity = 0;
                }
                //if tom is grounded and not going to hit anything and is not running slow him down
                if (!Tom.IsRunning && Tom.Grounded())
                {
                    if(Tom.horizontalVelocity < 0)
                    {
                        Tom.horizontalVelocity += .025;
                    }
                    if(Tom.horizontalVelocity > 0)
                    {
                        Tom.horizontalVelocity -= .025;
                    }
                }
                //if tom is in the air and not going to hit anything and not running slow him down
                if (!Tom.IsRunning && !Tom.Grounded())
                {
                    if (Tom.horizontalVelocity < 0)
                    {
                        Tom.horizontalVelocity += .005;
                    }
                    if (Tom.horizontalVelocity > 0)
                    {
                        Tom.horizontalVelocity -= .005;
                    }
                }
                //if tom is almost stopped and not running just stop him (I hate doubles) and grounded
                if (!Tom.IsRunning && Tom.Grounded())
                {
                    if (Tom.horizontalVelocity < 0.025 && Tom.horizontalVelocity > 0)
                    {
                        Tom.horizontalVelocity = 0;
                    }
                    if (Tom.horizontalVelocity > -0.025 && Tom.horizontalVelocity < 0)
                    {
                        Tom.horizontalVelocity = 0;
                    }
                }

                //Move Tom
                Tom.Y = Tom.Y + Tom.verticalVelocity;
                Tom.X = Tom.X + Tom.horizontalVelocity;


                Thread.Sleep(1);
            }
        }


        //**********************************************************************************************\\
        //                                    not sure but necessary                                    \\
        //______________________________________________________________________________________________\\
        public ObservableCollection<MajorTomVMB> MajorToms { get; set; }
        public ObservableCollection<PlatformVMB> Platforms { get; set; }
        public ObservableCollection<PlatformVMB> Platform { get; set; }
        public testCommand jump { get; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZGF.Revit.Strings
{
    class ScaleToString
    {
        
        public static string GetScaleString(int ScaleValue)
        {
            string theScale = ScaleValue.ToString();

            switch (ScaleValue)
            {
                case 1:
                    theScale = "1:1";
                    break;
                case 2:
                    theScale = "1:2";
                    break;
                case 4:
                    theScale = "3\" = 1'-0\"";
                    break;
                case 5:
                    theScale = "1:5";
                    break;
                case 8:
                    theScale = "1-1/2\" = 1'-0\"";
                    break;
                case 10:
                    theScale = "1:10";
                    break;
                case 12:
                    theScale = "1\" = 1'-0\"";
                    break;
                case 16:
                    theScale = "3/4\" = 1'-0\"";
                    break;
                case 20:
                    theScale = "1:20";
                    break;
                case 24:
                    theScale = "1/2\" = 1'-0\"";
                    break;
                case 25:
                    theScale = "1:25";
                    break;
                case 32:
                    theScale = "3/8\" = 1'-0\"";
                    break;
                case 48:
                    theScale = "1/4\" = 1'-0\"";
                    break;
                case 50:
                    theScale = "1:50";
                    break;
                case 60:
                    theScale = "1\" = 5'";
                    break;
                case 64:
                    theScale = "3/16\" = 1'-0\"";
                    break;
                case 96:
                    theScale = "1/8\" = 1'-0\"";
                    break;
                case 100:
                    theScale = "1:100";
                    break;
                case 120:
                    theScale = "1\" = 10'";
                    break;
                case 128:
                    theScale = "3/32\" = 1'-0\"";
                    break;
                case 192:
                    theScale = "1/16\" = 1'-0\"";
                    break;
                case 200:
                    theScale = "1:200";
                    break;
                case 240:
                    theScale = "1\" = 20'";
                    break;
                case 250:
                    theScale = "1:250";
                    break;
                case 360:
                    theScale = "1\" = 30'";
                    break;
                case 384:
                    theScale = "1/32\" = 1'-0\"";
                    break;
                case 480:
                    theScale = "1\" = 40'";
                    break;
                case 500:
                    theScale = "1:500";
                    break;
                case 600:
                    theScale = "1\" = 50'";
                    break;
                case 768:
                    theScale = "1/64\" = 1'-0\"";
                    break;
                case 960:
                    theScale = "1\" = 80'";
                    break;
                case 1000:
                    theScale = "1:1000";
                    break;
                case 1200:
                    theScale = "1\" = 100'";
                    break;
                case 2400:
                    theScale = "1\" = 200'";
                    break;
                case 6000:    
                    theScale = "1\" = 500'";
                    break;
                case 12000:
                    theScale = "1\" = 1000'";
                    break;
                default:
                    theScale = ScaleValue.ToString();
                    break;
            }

            return theScale;
        }



    }



}

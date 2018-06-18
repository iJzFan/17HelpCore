using HELP.Service.ViewModel.Shared;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Text;
using System.Drawing.Imaging;
using System.Drawing;

namespace HELP.UI.Responsible.WebHelp
{
    public class ImageCodeHelper
    {

        #region 构造函数
        //private readonly IHttpContextAccessor _httpContextAccessor;
        //public ISession _session => _httpContextAccessor.HttpContext.Session;

        public const string SESSION_IMAGE_CODE = "ValidateCode";

        //public ImageCodeHelper(IHttpContextAccessor httpContextAccessor)
        //{
            //_httpContextAccessor = httpContextAccessor;
        //}

        #endregion


        /// <summary>  
        /// 该方法用于生成指定位数的随机数  
        /// </summary>  
        /// <param name="VcodeNum">参数是随机数的位数</param>  
        /// <returns>返回一个随机数字符串</returns>  
        private string RndNum(int VcodeNum)
        {
            //验证码可以显示的字符集合  
            string Vchar = "0,1,2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,i,j,k,l,m,n,p" +
                ",q,r,s,t,u,v,w,x,y,z,A,B,C,D,E,F,G,H,I,J,K,L,M,N,P,P,Q" +
                ",R,S,T,U,V,W,X,Y,Z";
            string[] VcArray = Vchar.Split(new Char[] { ',' });//拆分成数组   
            string code = "";//产生的随机数  
            int temp = -1;//记录上次随机数值，尽量避避免生产几个一样的随机数  

            Random rand = new Random();
            //采用一个简单的算法以保证生成随机数的不同  
            for (int i = 1; i < VcodeNum + 1; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(i * temp * unchecked((int)DateTime.Now.Ticks));//初始化随机类  
                }
                int t = rand.Next(61);//获取随机数  
                if (temp != -1 && temp == t)
                {
                    return RndNum(VcodeNum);//如果获取的随机数重复，则递归调用  
                }
                temp = t;//把本次产生的随机数记录起来  
                code += VcArray[t];//随机数的位数加一  
            }
            return code;
        }


        /// <summary>  
        /// 该方法是将生成的随机数写入图像文件  
        /// </summary>  
        /// <param name="code">code是一个随机数</param>
        /// <param name="numbers">生成位数（默认4位）</param>  
        public static byte[] CreateValidateGraphic(string validatecode, int numbers = 4)
        {
            //code = RndNum(numbers);
            Bitmap Img = null;
            Graphics g = null;
            MemoryStream ms = null;
            Random random = new Random();
            //验证码颜色集合  
            Color[] c = { Color.Black, Color.Red, Color.DarkBlue, Color.Green, Color.Orange, Color.Brown, Color.DarkCyan, Color.Purple };

            //验证码字体集合
            string[] fonts = { "Verdana", "Microsoft Sans Serif", "Comic Sans MS", "Arial", "宋体" };


            //定义图像的大小，生成图像的实例  
            Img = new Bitmap((int)validatecode.Length * 18, 32);

            g = Graphics.FromImage(Img);//从Img对象生成新的Graphics对象    

            g.Clear(Color.White);//背景设为白色  

            //在随机位置画背景点  
            for (int i = 0; i < 100; i++)
            {
                int x = random.Next(Img.Width);
                int y = random.Next(Img.Height);
                g.DrawRectangle(new Pen(Color.LightGray, 0), x, y, 1, 1);
            }
            //验证码绘制在g中  
            for (int i = 0; i < validatecode.Length; i++)
            {
                int cindex = random.Next(7);//随机颜色索引值  
                int findex = random.Next(5);//随机字体索引值  
                Font f = new Font(fonts[findex], 15, FontStyle.Bold);//字体  
                Brush b = new SolidBrush(c[cindex]);//颜色  
                int ii = 4;
                if ((i + 1) % 2 == 0)//控制验证码不在同一高度  
                {
                    ii = 2;
                }
                g.DrawString(validatecode.Substring(i, 1), f, b, 3 + (i * 12), ii);//绘制一个验证字符  
            }
            ms = new MemoryStream();//生成内存流对象  
            Img.Save(ms, ImageFormat.Jpeg);//将此图像以Png图像文件的格式保存到流中  

            //回收资源  
            g.Dispose();
            Img.Dispose();
            return ms.ToArray();
        }


        /// <summary>
        /// Generate random identifying code
        /// </summary>
        /// <param name="VcodeNum">length of the identifying code</param>
        /// <returns>identifying code</returns>
        public static string CreateValidateCode(int VcodeNum)
        {
            string Vchar = "23456789ABCDEFGHJKLMNPQRSTUVWXYZ";
            int len = Vchar.Length; // the whole collection length
            int codeLen = VcodeNum;
            int i = 0; ;
            StringBuilder idenCode = new StringBuilder();
            System.Random random = new Random();
            int next = -1;
            string picked = string.Empty;
            do
            {
                next = random.Next(0, len - 1);
                picked = Vchar.Substring(next, 1);
                if (idenCode.ToString().IndexOf(picked) == -1)
                {
                    i++;
                    idenCode.Append(picked);
                }
            } while (i < codeLen);
            return idenCode.ToString();
        }


        /// <summary>
        /// sinusoid twist
        /// </summary>
        /// <param name="srcBmp"></param>
        /// <param name="bXDir">whether to twist</param>
        /// <param name="dMultValue">Mult Value usually < 3 </param>
        /// <param name="dPhase">the start phase value, usually [0 - 2*Math.PI]</param>
        /// <returns></returns>
        private static Bitmap TwistImage(Bitmap srcBmp, bool bXDir, double dMultValue, double dPhase)
        {

            Bitmap destBmp = new Bitmap(srcBmp.Width, srcBmp.Height);
            Graphics graph = Graphics.FromImage(destBmp);
            graph.FillRectangle(new SolidBrush(Color.White), 0, 0, destBmp.Width, destBmp.Height);
            graph.Dispose();
            double dBaseAxisLen = bXDir ? Double.Parse(destBmp.Height.ToString()) : Double.Parse(destBmp.Width.ToString());
            for (int i = 0; i <= destBmp.Width - 1; i++)
            {
                for (int j = 0; j <= destBmp.Height - 1; j++)
                {
                    double dx = 0;
                    dx = bXDir ? Math.PI * 2 * Double.Parse(j.ToString()) / dBaseAxisLen : Math.PI * 2 * Double.Parse(i.ToString()) / dBaseAxisLen;
                    dx += dPhase;
                    double dy = Math.Sin(dx);

                    // get the color of the current point
                    int nOldX = 0;
                    int nOldY = 0;
                    nOldX = bXDir ? i + Convert.ToInt32(dy * dMultValue) : i;
                    nOldY = bXDir ? j : j + Convert.ToInt32(dy * dMultValue);

                    Color color = srcBmp.GetPixel(i, j);
                    if (nOldX >= 0 && nOldX < destBmp.Width && nOldY >= 0 && nOldY < destBmp.Height)
                    {
                        destBmp.SetPixel(nOldX, nOldY, color);
                    }
                }
            }
            return destBmp;
        }


        #region ImageCode

        /// <summary>
        /// Check if the input image code is same with the value in the session
        /// </summary>
        /// <param name="inputCode"></param>
        /// <returns></returns>
        private static ImageCodeError CheckImageCode(string inputImageCode,string sessionImagaeCode)
        {

            if (sessionImagaeCode == null)
            {
                return ImageCodeError.Expired;
            }
            else if (sessionImagaeCode.ToString().Trim() != inputImageCode.ToUpper())
            {
                return ImageCodeError.Wrong;
            }
            else
            {
                return ImageCodeError.NoError;
            }

        }

        internal static ImageCodeModel CheckResult(ImageCodeModel model,string sessionImageCode)
        {

           model.ImageCodeError = CheckImageCode(model.InputImageCode, sessionImageCode);

           return model;
        }

        #endregion
    }
}

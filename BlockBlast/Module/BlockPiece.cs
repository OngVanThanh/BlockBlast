using System.Collections.Generic;
using System.Drawing;

namespace BlockBlast
{
    public class BlockPiece
    {
        public int[,] Shape { get; private set; }
        public int ColorIndex { get; private set; }

        public BlockPiece(int[,] shape, int colorIndex)
        {
            Shape = shape;
            ColorIndex = colorIndex;
        }

        // Danh sách các hình dạng khối gạch chuẩn trong game
        public static readonly List<int[,]> AllShapes = new List<int[,]>()
        {
            new int[,] { {1} },                                       // Ô đơn 1x1
            new int[,] { {1, 1} },                                    // Thanh ngang 2
            new int[,] { {1, 1, 1} },                                 // Thanh ngang 3
            new int[,] { {1}, {1}, {1} },                             // Thanh dọc 3
            new int[,] { {1, 1}, {1, 1} },                            // Khối vuông 2x2
            new int[,] { {1, 1, 1}, {1, 1, 1}, {1, 1, 1} },          // Khối vuông 3x3
            new int[,] { {1, 0}, {1, 0}, {1, 1} },                    // Chữ L
            new int[,] { {1, 1, 1}, {0, 1, 0} }                       // Chữ T
        };

        // Bảng màu cho từng chỉ số
        public static Color GetColor(int colorIndex)
        {
            switch (colorIndex)
            {
                case 1: return Color.FromArgb(231, 76, 60);   // Đỏ
                case 2: return Color.FromArgb(46, 204, 113);  // Xanh lá
                case 3: return Color.FromArgb(52, 152, 219);  // Xanh dương
                case 4: return Color.FromArgb(241, 196, 15);  // Vàng
                case 5: return Color.FromArgb(155, 89, 182);  // Tím
                case 6: return Color.FromArgb(230, 126, 34);  // Cam
                case 7: return Color.FromArgb(26, 188, 156);  // Xanh ngọc
                default: return Color.FromArgb(40, 44, 68);   // Ô trống trên bàn chơi
            }
        }
    }
}
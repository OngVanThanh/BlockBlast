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

        // Danh sách các hình dạng khối gạch chuẩn trong game (~20+ khối kinh điển)
        public static readonly List<int[,]> AllShapes = new List<int[,]>()
        {
            // 1. Khối đơn 1x1
            new int[,] { { 1 } },

            // 2. Thanh ngang và dọc (1x2, 2x1, 1x3, 3x1, 1x4, 4x1, 1x5, 5x1)
            new int[,] { { 1, 1 } },
            new int[,] { { 1 }, { 1 } },
            new int[,] { { 1, 1, 1 } },
            new int[,] { { 1 }, { 1 }, { 1 } },
            new int[,] { { 1, 1, 1, 1 } },
            new int[,] { { 1 }, { 1 }, { 1 }, { 1 } },
            new int[,] { { 1, 1, 1, 1, 1 } },
            new int[,] { { 1 }, { 1 }, { 1 }, { 1 }, { 1 } },

            // 3. Khối vuông (2x2, 3x3)
            new int[,] { { 1, 1 }, { 1, 1 } },
            new int[,] { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 } },

            // 4. Khối góc mini 2x2 (chữ L 3 ô) xoay 4 hướng
            new int[,] { { 1, 1 }, { 1, 0 } },
            new int[,] { { 1, 1 }, { 0, 1 } },
            new int[,] { { 1, 0 }, { 1, 1 } },
            new int[,] { { 0, 1 }, { 1, 1 } },

            // 5. Chữ L lớn 3x3 xoay 4 hướng
            new int[,] { { 1, 0, 0 }, { 1, 0, 0 }, { 1, 1, 1 } },
            new int[,] { { 0, 0, 1 }, { 0, 0, 1 }, { 1, 1, 1 } },
            new int[,] { { 1, 1, 1 }, { 1, 0, 0 }, { 1, 0, 0 } },
            new int[,] { { 1, 1, 1 }, { 0, 0, 1 }, { 0, 0, 1 } },

            // 6. Chữ T xoay 4 hướng
            new int[,] { { 1, 1, 1 }, { 0, 1, 0 } },
            new int[,] { { 0, 1, 0 }, { 1, 1, 1 } },
            new int[,] { { 1, 0 }, { 1, 1 }, { 1, 0 } },
            new int[,] { { 0, 1 }, { 1, 1 }, { 0, 1 } },

            // 7. Chữ Z và S (2x3 hoặc 3x2)
            new int[,] { { 1, 1, 0 }, { 0, 1, 1 } },
            new int[,] { { 0, 1, 1 }, { 1, 1, 0 } },
            new int[,] { { 0, 1 }, { 1, 1 }, { 1, 0 } },
            new int[,] { { 1, 0 }, { 1, 1 }, { 0, 1 } }
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
                default: return Color.FromArgb(28, 32, 48);   // Ô trống trên bàn chơi (#1C2030 - Dark Slate)
            }
        }
    }
}
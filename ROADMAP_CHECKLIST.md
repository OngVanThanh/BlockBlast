# 📋 BLOCK BLAST - MASTER ROADMAP & CHECKLIST NÂNG CẤP TOÀN DIỆN

> **Mục tiêu:** Nâng cấp dự án Block Blast từ một game giải đố cơ bản thành một siêu phẩm xếp gạch độc đáo, giao diện Dark Theme hiện đại, hiệu ứng mượt mà và tích hợp **cả 3 cơ chế gameplay đột phá**: **Elemental Blast**, **Rogue-lite Relics**, và **Bomb Defusal**.

---

## 📊 TIẾN ĐỘ TỔNG QUAN

- [x] **Giai đoạn 1:** Khắc phục Lỗi Cốt lõi & Chuẩn hóa Kiến trúc (Core Bug Fixes & Refactoring)
- [x] **Giai đoạn 2:** Lột xác UI/UX & Tăng cảm giác tương tác (Visual & Game Feel)
- [ ] **Giai đoạn 3:** Hệ thống Lưu trữ, Âm thanh & Quản lý Điểm Kỷ lục (Audio & High Score)
- [ ] **Giai đoạn 4:** Triển khai Bộ 3 Cơ Chế Đột Phá (Triple Innovation Gameplay)
  - [ ] 4.1 Chế độ 1: **Elemental Blast** (Khối Nguyên Tố & Phản Ứng Combo)
  - [ ] 4.2 Chế độ 2: **Rogue-lite Relics** (Bùa Chú, Hold Slot & Kỹ Năng)
  - [ ] 4.3 Chế độ 3: **Bomb Defusal** (Gỡ Bom Đếm Lùi Nước Đi)
  - [ ] 4.4 Menu Chọn Chế Độ Chơi (Game Mode Selector)
- [ ] **Giai đoạn 5:** Kiểm thử Hiệu năng, Đóng gói & Hoàn thiện (Testing & Polish)

---

## 🛠️ CHI TIẾT CÁC NHIỆM VỤ CỤ THỂ

### 🎯 GIAI ĐOẠN 1: FIX BUG CỐT LÕI & CHUẨN HÓA MÃ NGUỒN
 
- [x] **1.1 Đồng bộ công thức toạ độ chuột (Fix lệch tâm khi kéo thả):**
  - [x] Viết hàm chuẩn `BoardRenderer.GetCellFromPoint(Point pt, Size panelSize, out int row, out int col)` tính chính xác theo `MARGIN`, `GAP`, và `CalculateCellSize()`.
  - [x] Thay thế hoàn toàn cách tính thô `tileSize = pnlBoard.Width / 8` trong `FormGamePlay.cs`.
- [x] **1.2 Ngẫu nhiên hóa khối gạch & mở rộng kho hình khối:**
  - [x] Thêm đủ bộ 20+ hình khối chuẩn vào `BlockPiece.AllShapes` (thanh dài 4-5 ô, chữ Z, chữ S, chữ U, góc chữ L các chiều, khối 3x3, 1x1...).
  - [x] Sửa hàm `GameManager.CreateRandomPiece()`: bốc ngẫu nhiên từ `AllShapes` và chọn màu ngẫu nhiên (1 -> 7).
- [x] **1.3 Triệt tiêu nguy cơ treo UI Thread:**
  - [x] Xóa bỏ toàn bộ `Thread.Sleep(300)` và `Application.DoEvents()` trong quá trình chuyển màn chơi.
  - [x] Chuyển sang cơ chế bất đồng bộ `async/await Task.Delay()` hoặc dùng `System.Windows.Forms.Timer`.
- [x] **1.4 Dọn dẹp dữ liệu thừa:**
  - [x] Xóa mảng `private int[,] grid` và hàm `GetGrid()` không dùng trong `GameManager.cs`.
  - [x] Đảm bảo toàn bộ luồng game chỉ sử dụng duy nhất `gameManager.Board`.
- [x] **1.5 Nối toàn bộ sự kiện cho Controls:**
  - [x] Nối nút `btnReplay`: reset game, sinh gạch mới, ẩn bảng Game Over.
  - [x] Nối nút `btnPause`: đổi trạng thái `IsPaused`, chặn thao tác chuột khi đang tạm dừng.
  - [x] Nối nút `btnExit`: xác nhận và đóng ứng dụng an toàn.

---

### 🎨 GIAI ĐOẠN 2: LỘT XÁC UI/UX & GAME FEEL (CẢM GIÁC CHƠI ĐÃ TAY)

- [x] **2.1 Bảng màu Dark Cyberpunk / Modern Slate:**
  - [x] Nền Form và Panel: Chuyển sang màu tối sang trọng (`#131722` hoặc `#1A1D2E`).
  - [x] Các ô lưới cờ trống: Vẽ màu nền nổi nhẹ bo góc (`#22273B`).
  - [x] Khối gạch: Vẽ viền bóng nhẹ (Glow/Bevel) giúp khối có cảm giác nổi 3D.
- [x] **2.2 Cải tiến trải nghiệm Kéo - Thả (Smart Drag UX):**
  - [x] Nâng khối gạch lên cao hơn vị trí chuột 40-50px khi nhấp giữ để ngón tay/chuột không che tầm nhìn bàn cờ.
  - [x] Thêm hiệu ứng phát sáng mờ (Line Preview Glow) cho các hàng/cột sắp được nổ nếu người chơi thả gạch vào vị trí hiện tại.
- [x] **2.3 Hoạt họa nổ dòng (Line Clear Animation):**
  - [x] Khi có dòng đầy, các ô chớp sáng màu trắng trong 100ms $\rightarrow$ thu nhỏ và tan biến.
  - [x] Thêm popup chữ nổi trôi lên màn hình: `+100`, `COMBO x2!`, `MEGA BLAST!`.
- [x] **2.4 Rung màn hình (Screen Shake):**
  - [x] Khi ăn combo $\ge 2$ hàng, bàn cờ rung nhẹ ngẫu nhiên 3-5px trong 150ms tạo cảm giác uy lực.

---

### 💎 GIAI ĐOẠN 2.5: TINH CHỈNH GIAO DIỆN & KHẮC PHỤC LỖI BỐ CỤC

- [x] **2.5.1 Tự động lưu ngầm HighScore:**
  - [x] Loại bỏ `txtName`, `btnSave`, `lblInputGuide` khỏi bảng Game Over.
  - [x] Lưu và tải điểm cao nhất ngầm vào file `highscore.dat` tại ApplicationData.
- [x] **2.5.2 Chuẩn hóa Bàn cờ Hình Vuông & Căn Giữa Layout:**
  - [x] Đưa `pnlBoard` về hình vuông chuẩn (`CalculateCellSize` và `GetBoardPixelSize`).
  - [x] Căn giữa bàn cờ tuyệt đối theo chiều ngang và dọc (giữa pnlHeader và pnlDock).
  - [x] Tự động tính toán lại vị trí khi thay đổi kích thước cửa sổ (`Resize` / `Maximized`).
- [x] **2.5.3 Cân đối Thanh Dock (Empty Slot Placeholder):**
  - [x] Luôn hiển thị cả 3 ô Dock (`pnlDockPiece1, 2, 3`), kích thước 140x140px, khoảng cách 30px, tổng 480px căn giữa dock.
  - [x] Ô hết gạch (`Tag == null`) hoặc đang được kéo hiển thị viền ô trống mờ `Empty Slot Placeholder` (nền `#1A1D2E`, viền đứt `#2D324D`).
  - [x] Chỉ sinh 3 khối mới khi cả 3 ô dock đều trống (`Tag == null`).
- [x] **2.5.4 Thiết kế lại Bảng GAME OVER Hiện Đại:**
  - [x] Bảng `pnlGameOver` phủ trọn 100% diện tích `pnlBoard`, nền tối bán trong suốt `#121520` (Alpha 240).
  - [x] Tiêu đề `GAME OVER` to 24pt cam neon `#FF9F43`, điểm số trắng 16pt bold.
  - [x] Nhãn `lblBestRecord`: Hiển thị `"🎉 KỶ LỤC MỚI: {score} 🎉"` màu vàng neon nếu phá kỷ lục, hoặc `"KỶ LỤC: {score}"` màu xám bạc.
  - [x] Nút `btnReplay` to 180x50px bo góc 14px cam neon, chữ `"▶ CHƠI LẠI"`.

---

### 💾 GIAI ĐOẠN 3: LƯU TRỮ DỮ LIỆU, ÂM THANH & ĐIỂM CAO

- [x] **3.1 Quản lý Điểm Kỷ Lục (High Score System):**
  - [x] Lưu và tải `HighScore` ngầm tại `%AppData%/BlockBlast/highscore.dat`.
  - [x] Tự động cập nhật `lblBestScore` mỗi khi điểm hiện tại vượt qua điểm cao nhất.
  - [x] Tự động ghi nhận kỷ lục mới và hiển thị huy hiệu trên bảng GameOver mà không cần nhập tên thủ công.
- [x] **3.2 Hệ thống Âm thanh Đa âm điệu (SFX Procedural Engine):**
  - [x] Tích hợp `SoundManager` tự tổng hợp PCM WAV trực tiếp trong bộ nhớ bằng `System.Media.SoundPlayer`:
    - `sfx_pickup`: Tiếng "pop" nhanh vui tai khi nhấc gạch (520Hz -> 780Hz).
    - `sfx_drop`: Tiếng "cộp" đầm chắc của khối gỗ khi đặt gạch xuống bàn (180Hz decaying).
    - `sfx_combo`: Nốt nhạc thăng tiến dần (Đô C5 -> Mi E5 -> Sol G5 -> Đô C6) theo cấp độ Combo khi nổ dòng.
    - `sfx_gameover`: Tiếng trượt trầm buồn khi kết thúc ván chơi (420Hz -> 180Hz).
  - [x] Nối nút `btnSound` để bật/tắt toàn bộ âm thanh (Mute/Unmute: "🔊 BẬT" / "🔇 TẮT") và lưu cài đặt ngầm vào `%AppData%/BlockBlast/settings.dat`.

---

### 🚀 GIAI ĐOẠN 4: TRIỂN KHAI BỘ 3 CƠ CHẾ ĐỘT PHÁ (TRIPLE INNOVATION HOOKS)

- [ ] **4.0 Hệ thống Chọn Chế Độ Chơi (Game Mode Selector):**
  - [ ] Thêm Menu/Button chuyển đổi linh hoạt:
    1. Chế độ **Cổ Điển & Hướng Dẫn** (Classic & Puzzle Tutorial)
    2. Chế độ **Nguyên Tố** (Elemental Blast)
    3. Chế độ **Bùa Chú Roguelike** (Rogue-lite Relics)
    4. Chế độ **Gỡ Bom Nhanh** (Bomb Defusal)

---

#### 🌟 4.1 CHẾ ĐỘ 1: ELEMENTAL BLAST (KHỐI NGUYÊN TỐ)
- [ ] **Dữ liệu Nguyên Tố:**
  - [ ] Mở rộng `BlockPiece` thêm thuộc tính `ElementType`:
    - `Fire` (Hỏa - Đỏ rực)
    - `Lightning` (Lôi - Vàng sét)
    - `Ice` (Băng - Xanh tuyết)
    - `Rainbow` (Cầu vồng - Đa sắc)
  - [ ] Render biểu tượng icon nhỏ (🔥, ⚡, ❄️, ⭐) trên từng viên gạch.
- [ ] **Kỹ năng Kích nổ khi ăn dòng:**
  - [ ] **🔥 Hỏa (Fire):** Gây nổ lan (Splash Damage) phá hủy toàn bộ vùng 8 ô xung quanh ô lửa.
  - [ ] **⚡ Lôi (Lightning):** Bắn sét phá hủy ngẫu nhiên 2 - 3 ô rác còn sót lại trên bàn cờ.
  - [ ] **❄️ Băng (Ice):** Cần nổ 2 lần mới vỡ, nhưng nhân gấp **x3 điểm số**.
  - [ ] **⭐ Cầu vồng (Rainbow):** Khối thần kỳ tự động lấp đầy ô trống bất kỳ để hoàn thành dòng.
- [ ] **Phản ứng Nguyên Tố (Combo Reactions):**
  - [ ] Nổ cùng lúc Hỏa + Lôi = **Quá Tải (Overload)** $\rightarrow$ Dọn sạch 50% bàn cờ.
  - [ ] Nổ cùng lúc Hỏa + Băng = **Bốc Hơi (Vaporize)** $\rightarrow$ Thưởng điểm khổng lồ x5.

---

#### 🧲 4.2 CHẾ ĐỘ 2: ROGUE-LITE RELICS (BÙA CHÚ & KỸ NĂNG)
- [ ] **Ô Chứa Dự Trữ (Hold Slot):**
  - [ ] Bổ sung 1 Panel `pnlHold` ở góc màn hình. Cho phép kéo 1 khối gạch khó vào cất giữ và lấy ra dùng khi cần.
- [ ] **Lượt Đổi Khối (Reroll Button):**
  - [ ] Nút đổi cả 3 khối ở Dock thành 3 khối mới (hồi lại sau mỗi 8 nước đi hoặc 500 điểm).
- [ ] **Hệ thống Chọn Bùa Chú (Perks Modal):**
  - [ ] Khi đạt mốc điểm hoặc dọn sạch bàn cờ (Full Clear), mở popup chọn 1 trong 3 bùa:
    - 🧲 *Nam châm trọng lực:* Dồn toàn bộ các ô lẻ tẻ rơi xuống đáy.
    - 🔨 *Búa đập ô:* Cho phép nhấp click phá hủy trực tiếp 1 ô cản trở.
    - 🛡️ *Tái sinh:* Khi bị Game Over được dọn sạch nửa bàn cờ và tiếp tục chơi.

---

#### 💣 4.3 CHẾ ĐỘ 3: TIME-ATTACK BOMB DEFUSAL (GỠ BOM ĐẾM NGƯỢC)
- [ ] **Cơ chế Khối Bom:**
  - [ ] Cứ sau mỗi vài lượt, trên bàn cờ xuất hiện ngẫu nhiên một ô mang biểu tượng **Bom 💣** kèm số đếm ngược (ví dụ: `5` lượt).
  - [ ] Mỗi nước đi người chơi đặt gạch xuống, số đếm lùi giảm đi 1: `5 -> 4 -> 3 -> 2 -> 1 -> 0`.
- [ ] **Hậu quả khi Bom nổ:**
  - [ ] Nếu đếm về `0` mà người chơi chưa nổ được dòng chứa bom $\rightarrow$ Bom biến thành **Đá Cứng (Obsidian/Stone Block)**: Không thể xóa bằng cách thông thường, làm thu hẹp không gian chơi.
- [ ] **Phần thưởng khi gỡ bom thành công:**
  - [ ] Nổ được dòng chứa bom trước khi hết giờ $\rightarrow$ Bom bị vô hiệu hóa và thưởng ngay `+300 điểm`.

---

### 🧪 GIAI ĐOẠN 5: KIỂM THỬ, TỐI ƯU & XUẤT BẢN

- [ ] **5.1 Tối ưu hóa GDI+ & Bộ nhớ:**
  - [ ] Kiểm tra toàn bộ `Brush`, `Pen`, `GraphicsPath` được giải phóng bằng `using`.
  - [ ] Đảm bảo không bị rò rỉ RAM (Memory Leak) dù chơi liên tục hàng giờ.
- [ ] **5.2 Kiểm thử màn chơi & biên cờ:**
  - [ ] Kiểm thử va chạm tại các góc biên (hàng 0, 7; cột 0, 7).
  - [ ] Kiểm thử tính toán Game Over chuẩn xác, không bị báo thua sai.
- [ ] **5.3 Đóng gói (Release Build):**
  - [ ] Chuyển cấu hình sang `Release | AnyCPU`.
  - [ ] Build file `BlockBlast.exe` độc lập, sẵn sàng chơi ngay.

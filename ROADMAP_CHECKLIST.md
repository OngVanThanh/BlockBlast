# 📋 BLOCK BLAST - MASTER ROADMAP & CHECKLIST NÂNG CẤP TOÀN DIỆN

> **Mục tiêu:** Nâng cấp dự án Block Blast từ một game giải đố cơ bản thành một siêu phẩm xếp gạch độc đáo, giao diện Dark Theme hiện đại, hiệu ứng mượt mà và tích hợp **cả 3 cơ chế gameplay đột phá**: **Elemental Blast**, **Rogue-lite Relics**, và **Bomb Defusal**.

---

## 📊 TIẾN ĐỘ TỔNG QUAN

- [x] **Giai đoạn 1:** Khắc phục Lỗi Cốt lõi & Chuẩn hóa Kiến trúc (Core Bug Fixes & Refactoring)
- [x] **Giai đoạn 2:** Lột xác UI/UX & Tăng cảm giác tương tác (Visual & Game Feel)
- [x] **Giai đoạn 3:** Hệ thống Lưu trữ, Âm thanh & Quản lý Điểm Kỷ lục (Audio & High Score)
- [x] **Giai đoạn 4:** Thuật toán Thích ứng DDA & Phân chia 2 Chế độ chơi (Endless & Story Campaign)
- [x] **Giai đoạn 5:** Hệ thống Menu Chính, Modal Popups, Profile & Bảng Thành Tích (UI & Leaderboard)
- [x] **Giai đoạn 6:** Xử lý Ngoại lệ Toàn diện & Đóng gói (Testing, Exception Handling & Build 0 Error)
- [x] **Giai đoạn 7:** Kho Lưu Trữ 8 Đại Mùa Sử Thi, Thuật Toán Inverse Solvable, Bảo Tàng Huân Chương 2 Tab & Tự Động Lưu Ván Đấu (Session Persistence)

---

## 🛠️ CHI TIẾT CÁC NHIỆM VỤ ĐÃ THỰC THI

### 🎯 GIAI ĐOẠN 1: FIX BUG CỐT LÕI & CHUẨN HÓA MÃ NGUỒN
- [x] **1.1 Đồng bộ công thức toạ độ chuột (Fix lệch tâm khi kéo thả):**
  - [x] Viết hàm chuẩn `BoardRenderer.TryGetCellAt()` và `GetMagneticCellAt()` tính chính xác theo `MARGIN`, `GAP`, và `CalculateCellSize()`.
  - [x] Thay thế hoàn toàn cách tính thô cũ trong `FormGamePlay.cs`.
- [x] **1.2 Ngẫu nhiên hóa khối gạch & mở rộng kho hình khối:**
  - [x] Thêm đủ bộ 20+ hình khối chuẩn vào `BlockPiece.AllShapes`.
  - [x] Sửa hàm `GameManager.CreateRandomPiece()`: bốc ngẫu nhiên từ `AllShapes` kết hợp DDA.
- [x] **1.3 Triệt tiêu nguy cơ treo UI Thread:**
  - [x] Xóa bỏ toàn bộ `Thread.Sleep(300)` và `Application.DoEvents()`.
  - [x] Chuyển sang cơ chế bất đồng bộ `async/await Task.Delay()`.
- [x] **1.4 Dọn dẹp dữ liệu thừa:**
  - [x] Xóa mảng `grid` thừa và dùng duy nhất `gameManager.Board`.
- [x] **1.5 Nối toàn bộ sự kiện cho Controls:**
  - [x] Nối nút `btnReplay`, `btnPause`, `btnExit` an toàn.

---

### 🎨 GIAI ĐOẠN 2: LỘT XÁC UI/UX & GAME FEEL
- [x] **2.1 Bảng màu Dark Cyberpunk / Modern Slate:**
  - [x] Nền Form và Panel tối sang trọng (`#10131C` và `#181C2B`).
  - [x] Render khối gạch 3D Gem Bevel nổi khối.
- [x] **2.2 Cải tiến trải nghiệm Kéo - Thả (Smart Drag UX):**
  - [x] Nâng khối gạch lên cao 35px khi nhấp giữ (`LIFT_OFFSET_Y`).
  - [x] Layer `DragGhostWindow` mượt mà không che tầm nhìn bàn cờ.
- [x] **2.3 Hoạt họa nổ dòng (Line Clear Animation):**
  - [x] Chớp sáng trắng, thu nhỏ tan biến mượt mà.
  - [x] Chữ nổi trôi lên (`+100`, `COMBO x2!`, `MEGA BLAST!`).
- [x] **2.4 Rung màn hình (Screen Shake):**
  - [x] Rung màn hình ngẫu nhiên khi ăn combo $\ge 2$ dòng.

---

### 💎 GIAI ĐOẠN 2.5: TINH CHỈNH GIAO DIỆN & KHẮC PHỤC LỖI BỐ CỤC
- [x] **2.5.1 Tự động lưu ngầm HighScore:**
  - [x] Loại bỏ nhập tên thủ công khi Game Over. Tự động lưu ngầm vào file dữ liệu.
- [x] **2.5.2 Chuẩn hóa Bàn cờ Hình Vuông & Căn Giữa Layout:**
  - [x] Căn giữa bàn cờ và Dock tuyệt đối ở mọi kích thước màn hình và khi phóng to (Maximized).
- [x] **2.5.3 Cân đối Thanh Dock (Empty Slot Placeholder):**
  - [x] Luôn hiển thị 3 ô Dock cân đối; ô hết gạch hiển thị viền đứt mờ placeholder.
- [x] **2.5.4 Bảng Game Over Canvas Engine:**
  - [x] Chuyển vẽ Game Over sang trực tiếp trên canvas GDI+ của `pnlBoard`, xóa bỏ triệt để lỗi chớp nháy WinForms alpha.

---

### 💾 GIAI ĐOẠN 3: LƯU TRỮ DỮ LIỆU & ÂM THANH SFX
- [x] **3.1 Quản lý Điểm Kỷ Lục:**
  - [x] Tự động cập nhật `lblBestScore` và lưu vào `%AppData%/BlockBlast/highscore.dat`.
- [x] **3.2 Procedural SFX Engine (`SoundManager.cs`):**
  - [x] Tự sinh 100% sóng âm WAV (PCM) trong RAM:
    - Nhấc gạch (`sfx_pickup`: 520Hz -> 780Hz)
    - Thả gạch (`sfx_drop`: 180Hz gỗ)
    - Nổ dòng Combo theo thang âm (C5 -> E5 -> G5 -> C6)
    - Thua cuộc (`sfx_gameover`: trượt trầm)
  - [x] Nút `btnSound` bật/tắt âm thanh lưu ngầm vào `%AppData%/BlockBlast/settings.dat`.

---

### 🧠 GIAI ĐOẠN 4: THUẬT TOÁN THÍCH ỨNG DDA & PHÂN CHIA 2 CHẾ ĐỘ CHƠI (4.0đ)
- [x] **4.1 Thuật toán DDA Thích ứng (`DDAEngine.cs`):**
  - [x] Đo tỷ lệ lấp đầy bàn cờ trong thời gian thực (`CalculateBoardFillRatio`).
  - [x] Tình huống nguy cấp (>65% ô bị lấp): Sinh khối cứu hộ nhỏ gọn (1x1, 1x2, 2x1, 2x2, mini L).
  - [x] Tình huống thông thoáng (<35% ô bị lấp) & Cấp độ cao: Sinh khối phức tạp (3x3, 5x1, 1x5, Z, S, Big L).
  - [x] Tích hợp trực tiếp vào hàm `GameManager.CreateRandomPiece(difficulty)`.
- [x] **4.2 Chế độ Chơi Tự Do (Endless Classic):**
  - [x] Chơi tích điểm vô tận, DDA tự động tăng cấp theo điểm số (`1 + score / 400`).
  - [x] Lưu điểm kỷ lục cho từng người chơi.
- [x] **4.3 Chế độ Cốt Truyện Nguyên Tố (Story Campaign - `StoryManager.cs`):**
  - [x] Cốt truyện 4 Chương "Hàn Gắn Lõi Nguyên Tố":
    - **Chương 1:** Lõi Đất Mẹ (4 dòng / 15 nước đi)
    - **Chương 2:** Lõi Băng Giá (6 dòng / 18 nước đi)
    - **Chương 3:** Lõi Lửa Địa Ngục (8 dòng / 20 nước đi)
    - **Chương 4:** Lõi Sấm Sét (10 dòng / 22 nước đi)
  - [x] **Trải Nghiệm Storytelling & Animation Điện Ảnh (Hybrid Epic Intro):**
    - [x] Modal Briefing Giới Thiệu Cốt Truyện (`pnlStoryBriefingModal`): Huy hiệu nguyên tố phát sáng, lời dẫn sứ mệnh cổ đại, mục tiêu nhiệm vụ rõ ràng và nút `[⚡ TIẾN VÀO CHIẾN TRƯỜNG]`.
    - [x] Hiệu ứng Bàn cờ Thức tỉnh (`Awakening Wave Pulse`): Sóng năng lượng tỏa từ tâm bàn cờ theo màu nguyên tố + SFX kích hoạt trước khi mở quyền kéo gạch.
    - [x] Thanh Năng Lượng Lõi (Core Energy Bar): Hiển thị tiến trình trực quan `[■■■□□] %` ngay trên Banner bàn cờ.
    - [x] Viền bàn cờ đổi sắc theo nguyên tố và Floating Text nổ dòng mang âm hưởng cốt truyện (`🌿 ĐẤT MẸ THỨC TỈNH!`, `🔥 HỎA DIỆM BÙNG NỔ!`).
    - [x] Modal Vinh Danh Chiến Thắng (`pnlStoryVictoryModal`) & Thất Bại (`pnlStoryDefeatModal`) kèm lời dẫn chuyển giao câu chuyện sang chương sau.
  - [x] Quản lý Overlay tập trung (`HideAllOverlays()` / `PrepareModalDisplay()` / `ResumeGameplayUI()`): Không bao giờ bị kẹt modal hay lộ bàn cờ/header/dock.

---

### 🏆 GIAI ĐOẠN 5: MENU CHÍNH, LEADERBOARD & MODALS (3.0đ)
- [x] **5.1 Hệ thống Quản lý Người Chơi (`UserManager.cs`):**
  - [x] Lưu/đọc JSON danh sách người chơi vào `%AppData%/BlockBlast/users_data.json`.
  - [x] Quản lý `Username`, `BestScore`, `StoryChapterReached`, `LastPlayedDate`.
- [x] **5.2 Màn hình Menu Chính (`pnlMainMenu`):**
  - [x] Tiêu đề đồ họa hiện đại "BLOCK BLAST - ELEMENTAL PUZZLE & DDA EDITION".
  - [x] Profile Card hiển thị thông tin người chơi, kỷ lục cá nhân, tiến trình cốt truyện và nút `[ĐỔI TÊN]`.
  - [x] 5 nút hành động chính: Chơi Tự Do, Cốt Truyện, Bảng Thành Tích, Hướng Dẫn & Nhóm Tác Giả, Thoát.
- [x] **5.3 Modal Bảng Thành Tích (`pnlLeaderboardModal`):**
  - [x] Xếp hạng top người chơi theo điểm cao nhất kèm huy chương (🥇 Top 1 Vàng, 🥈 Top 2 Bạc, 🥉 Top 3 Đồng).
- [x] **5.4 Modal Giới Thiệu & Thông Tin Nhóm (`pnlAboutModal`):**
  - [x] Đầy đủ thông tin: Đề tài, Giảng viên hướng dẫn, 3 thành viên nhóm (Họ tên, MSSV, Đóng góp chi tiết), hướng dẫn luật chơi.
- [x] **5.5 Nút [🏠 MENU] In-Game:**
  - [x] Cho phép người chơi tạm dừng và quay về Menu Chính bất kỳ lúc nào.

---

### 🛡️ GIAI ĐOẠN 6: XỬ LÝ NGOẠI LỆ & BUILD (1.0đ)
- [x] **6.1 Bắt Ngoại Lệ 100%:**
  - [x] Toàn bộ thao tác I/O file, timers, audio, event handlers, rendering đều được bọc `try-catch` an toàn tuyệt đối.
- [x] **6.2 Sửa dứt điểm lỗi tương tác Game Over:**
  - [x] Khóa hoàn toàn chuột khi thua; tự động hủy kéo (`CancelDrag()`).
- [x] **6.3 Clean Build:**
  - [x] Biên dịch thành công: **0 Warning, 0 Error**.

---

### 👑 GIAI ĐOẠN 7: KHO LƯU TRỮ 8 ĐẠI MÙA SỬ THI, INVERSE SOLVABLE & PERSISTENCE
- [x] **7.1 Kho Lưu Trữ 8 Đại Mùa Sử Thi (`SeasonManager.cs` & `seasons_data.json`):**
  - [x] Cấu hình độc lập qua file `%AppData%/BlockBlast/seasons_data.json` (tự động fallback tạo mẫu khi chưa có file).
  - [x] 8 Đại Mùa Nguyên Tố (96 màn chơi thử thách):
    - **Mùa 1: Lục Địa Đất Mẹ** (🌿 Emerald `#2ECC71`)
    - **Mùa 2: Kỷ Băng Hà Vĩnh Cửu** (❄️ Cyan `#00D2D3`)
    - **Mùa 3: Tro Tàn Hỏa Ngục** (🔥 Crimson `#FF4757`)
    - **Mùa 4: Đỉnh Tháp Thiên Lôi** (⚡ Gold `#FFA502`)
    - **Mùa 5: Vực Thẳm Biển Sâu** (🌊 Deep Blue `#1E90FF`)
    - **Mùa 6: Bão Cát Sa Mạc** (🏜️ Amber `#E67E22`)
    - **Mùa 7: Hư Không Vũ Trụ** (🌌 Purple `#9B59B6`)
    - **Mùa 8: Thời Khắc Khải Huyền** (👑 Platinum `#F1F2F6`)
- [x] **7.2 Thuật Toán Sinh Thử Thách Động Mô Phỏng Ngược (`InverseChallengeGenerator.cs`):**
  - [x] Áp dụng nguyên lý toán học nghịch đảo (Inverse Simulation): bảo đảm 100% mọi màn đều có lời giải chiến thắng, loại bỏ tình trạng bế tắc ngẫu nhiên.
  - [x] Cân bằng độ khó tăng tiến rõ rệt từ Màn 1 đến Màn 12 của từng mùa.
- [x] **7.3 Phân Luồng Kiến Trúc Độc Lập OOP (`GameBoard.cs` & `GameChallenge.cs`):**
  - [x] `GameBoard.cs`: Quản lý ô cờ 8x8 và tính điểm vật lý.
  - [x] `GameChallenge.cs`: Đóng gói độc lập luật chơi, vật cản, mục tiêu dòng, giới hạn nước đi và điều kiện thắng/thua.
- [x] **7.4 Hub Băng Chuyền Mùa & Lưới Màn Chơi (Season Carousel & Level Grid):**
  - [x] Băng chuyền duyệt mượt mà qua 8 Mùa (`[◀]` và `[▶]`) với biểu tượng, lời dẫn sử thi và tiến độ `[■■■□□] X/12 Màn`.
  - [x] Lưới 12 nút màn chơi trực quan: `✓ Đã vượt`, `▶ Sẵn sàng`, `🔒 Đang khóa`.
- [x] **7.5 Nhật Ký Thành Tích & Bảo Tàng Huân Chương 2 Tab:**
  - [x] **Tab 1 (Bảo Tàng Huân Chương):** Trưng bày 8 Huân chương Đại Mùa + 4 Huy chương Cao thủ (Combo x4, Triệu Điểm, Bất Diệt, Đại Kiến Trúc Sư).
  - [x] **Tab 2 (Thống Kê Sự Nghiệp):** Điểm kỷ lục, tổng điểm trọn đời, tổng dòng nổ, chuỗi combo kỷ lục, cấp mùa cao nhất đã mở.
- [x] **7.6 Tự Động Lưu Trữ & Khôi Phục Ván Đấu Dở Dang (`SessionPersistence.cs`):**
  - [x] Tự động lưu trạng thái ván đấu vào `game_session.json` khi thoát app hoặc về Menu.
  - [x] Nút nổi bật `[▶ TIẾP TỤC VÁN CHƠI]` trên Menu Chính giúp khôi phục nguyên vẹn 100% bàn cờ và 3 khối gạch.
- [x] **7.7 Luồng Tân Thủ Thông Minh:**
  - [x] Tự động hướng dẫn người chơi mới khi `BestScore == 0`.
  - [x] Nút nổi bật `[⏩ BỎ QUA HƯỚNG DẪN (SKIP)]` giúp người chơi có thể chuyển thẳng vào Chế độ Chơi Tự Do bất cứ lúc nào.


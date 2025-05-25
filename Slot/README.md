# Deep
拉霸機系統

"    主要模組：
SlotMachineManager：遊戲流程總控

SlotSpinner：Slot 動畫與停止邏輯

SlotEvaluator：戰鬥判定與數值處理

AttackExecutor：傷害與特效執行

CardSlotSystem：玩家卡牌裝備槽



Slide 3：SlotMachineManager（流程總控）

功能：

控制戰鬥節奏與按鈕狀態

扣除金錢後觸發 SlotSpinner

結果傳給 SlotEvaluator 執行

亮點：

支援 ReSpin 一次機會

搭配動畫與音效協同演出

Slide 4：SlotSpinner（動畫控制）

功能：

控制 slot UI 滾動與停下

每格 slot 有獨立的速度與順序

支援加速重轉（ReSpin）

技術實現：

使用 Coroutine 控制位置變化

與 UI Animator 解耦合設計

Slide 5：SlotEvaluator（邏輯判定）

功能：

根據 slot 結果計算 combo 效果

敵人攻擊次數、玩家卡片出現次數統計

播放動畫、扣血與飛字顯示

特色：

支援 combo 強化（x2 / x3）

整合金錢消耗邏輯與轉動限制

Slide 6：AttackExecutor（傷害處理器）

功能：

根據卡牌 ID 與次數計算傷害

支援特殊效果如護盾、詛咒

播放音效、觸發 Shake/Hit 動畫

優點：

與 Slot 評估邏輯分離，方便維護

提供 Preview 計算供 UI 顯示

Slide 7：CardSlotSystem（裝備卡槽）

功能：

玩家於關卡前選擇 3 張卡

提供卡片 ID 給 SlotEvaluator 用於判定

模擬裝備制與卡牌策略性

應用彈性：

未來支援更多卡片類型只需擴充卡表
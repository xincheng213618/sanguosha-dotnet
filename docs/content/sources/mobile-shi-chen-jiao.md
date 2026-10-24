# 势·陈矫（mobile-shi-chen-jiao）来源与实现说明

## 官方来源

- 武将：势·陈矫（移动版"兵势篇"势包，魏，男，3 体力，定位控制，2025-11-12 上线，"2025年11月测试服"获取）。
- 技能文本核对（2026-10-24）：
  - BWIKI 页面 `https://wiki.biligame.com/msgs/势陈矫`（页面最后编辑 2025-11-24 21:41，旧版 id 34566，非爆料版标记，含完整当前版本技能文本与台词）。
  - 官网 `www.sanguosha.cn` 无此武将页面：官网英雄详情编号截至 675（邴原），势陈矫仍为测试服武将未正式收录，故本武将为 **BWIKI 单源**；萌娘百科词条 `https://zh.moegirl.org.cn/三国杀:陈矫`（2025-11-21 版本）旁证同一机制（措辞为"你将展示牌中最多颜色的所有手牌当一张不计入次数的任意【杀】对其使用"）。
- 技能原文（BWIKI 逐字）：
  - **清严**：你可以展示X张手牌（X为本轮此技能的发动次数且X至多为5），视为使用一张【闪】或者【无懈可击】。若如此做，此技能失效直至你的手牌中没有以此法展示的牌。
  - **策断**：出牌阶段限一次，你可以选择一名攻击范围内包含你的角色，其攻击范围内的所有角色同时展示一张手牌，然后你将手牌中展示牌最多颜色的所有牌当作一张不计入次数的【杀】对其使用；若造成伤害，你摸一张牌。

## 立绘

- BWIKI 文件页 `File:势陈矫-经典形象.png`，原图
  `https://patchwiki.biligame.com/images/msgs/6/69/f36ktt3no886poa6fx0cctg5r1rh411.png`
  （452×640 PNG，BWIKI 仅有此分辨率的官方经典形象）。等比缩放（452/640≈530/750，无裁剪）
  为 530×750 JPEG（LANCZOS，quality 88）存至 `src/CardGame.Wpf/Assets/mobile-shi-chen-jiao.jpg`
  （sha256 `d23389a5e5451e009fc1a8328c0401dd66049de475074adff10bbe455dc6d019`）。
  原图低于成品分辨率，属放大重采样；待官方放出高清原画后可替换（provisional upscale）。

## 实现说明

- registrationOrder：清严 51400、策断 51401、武将 51404；程序操作
  `ResolveChenJiaoQingYan = 63480`、`ResolveChenJiaoCeDuan = 63481`、
  `ResolveChenJiaoCeDuanSettle = 63482`；标记 `PlayerMarkerKind.ChenJiaoQingYanShown = 33700`
  （显示名"清严"，镜像当前手中以此法展示的牌数）；运行时钩子/校验优先级 68100。
- 清严：出牌阶段激活（usesPerPhase 空，由下述门槛自限）。X = min(本轮发动次数+1, 5)；
  `[ProgramActivationLegality(BeforeCondition)]` 在"技能失效中（有展示牌仍在手）或手牌数小于 X"时
  拦截。激活后逐张私密选择 X 张手牌展示（牌留在手中，记 `ChenJiaoQingYanShownEvent` 账本），
  再选择视为使用的【闪】/【无懈可击】：引擎无主动使用闪/无懈的结算路径，故以声明级虚拟使用提交
  （`CardUseDeclaredEvent` + 无实体 `CardUsedEvent`，记 `ChenJiaoQingYanUsedEvent`）。
  "失效直至手牌中没有以此法展示的牌"由账本按读推导（展示牌 id 当前仍在本人手区即失效），
  展示牌离手瞬间条件达成即自动解除；标记由 `ProgramRuleObserver` 随牌移动同步。
- 策断：出牌阶段限一次激活，目标 `otherLivingWhoseAttackRangeIncludesOwner`（数据驱动目标类别，
  环形座位两侧相邻角色均可选）。目标选定后其攻击范围内所有存活角色（含目标本人与陈矫）依次
  私密展示一张手牌（无手牌者跳过，记 `ChenJiaoCeDuanShownEvent`）；展示牌红黑各自计数，平局时
  由陈矫选择颜色，否则取多数。该颜色的所有展示牌先收至陈矫手区（`skill-program.mobile:shi-ceduan.take`，
  引擎用牌支付要求单一持有者），再按 inspected-hand 无实体程序【杀】蓝图对目标使用：
  `countsTowardSlashLimit` 不扣次数（不计入次数），距离仍按普通【杀】校验；
  攻击子帧经通用程序恢复路径回到程序帧，settle 指令对比用前伤害账本判定
  "若造成伤害，你摸一张牌"（`DrawProgramCards` 立即摸 1，记 `ChenJiaoCeDuanCompletedEvent`）。
  展示牌为空或使用被禁止时不使用【杀】而正常结束。
- 实现偏差（受引擎既有机制约束）：①清严的虚拟闪/无懈为声明级使用，不进入完整 CardUseFrame
  结算；②策断的展示牌经收用进入手区后再作为无实体程序【杀】的账本材料，卡牌直接关联通过
  `ChenJiaoCeDuanUseIssuedEvent.MaterialCount` 与移动账本呈现。
- 覆盖：清严 X 递增/锁止/再武装与两种虚拟牌、策断多座展示/多数颜色/平局选择/不计入次数/
  伤害摸牌/每阶段一次/目标合法性，各见 `ChenJiaoChecks` 三个专项检查（含冷恢复与命令重放）。

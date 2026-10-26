# 黄月英·移动版

核查日期：2026-10-26。普通移动身份，完整引用既有技能，不复制运行时或新增重复人物夹具。

### 黄月英：完整复用依据

- 当前[官网7](https://www.sanguosha.cn/pc/hero-detail-7.html)与移动Wiki经典身份一致。固定人物版本[34843](https://wiki.biligame.com/msgs/index.php?title=黄月英&oldid=34843)（2025-12-05）；技能[集智33207](https://wiki.biligame.com/msgs/index.php?title=集智/黄月英&oldid=33207)（2025-09-20）、[奇才6704](https://wiki.biligame.com/msgs/index.php?title=奇才/黄月英&oldid=6704)。这是已开服的普通标准身份，不能混入国战“非虚拟、非转化”条件。
- `src/Characters/_shared/Skills/passive-card-rules.rules.json`：集智普通锦囊使用走 `cardUseBeforeTargetEffects` 摸1；无懈可击另由真实 `cardResponseAccepted` 摸1；没有阶段/回合限次，没有界版“亮出基本牌可弃置＋上限”的额外分支。虚拟/转化用牌也未加国战排除。奇才使用既有锦囊距离许可，不解除目标禁止或其他用牌条件。
- 锦囊名单不是遗漏活动牌的证据：`Modes/Rules/ModeSkillProgramProjection.cs` 识别完整传统普通锦囊族，按当前模式投影新增/替换锦囊；集智的主动族含借刀杀人，满足完整族条件。原注册和GameplayHash保持原身份。
- 真实原生摸牌与代表cold已有 `Ol2016ShenSimaYiChecks.ActualPhaseDiscardsOriginalJiluePaidDrawAndLianpoCold` 的原版集智支路；活动牌投影已有 `ActivityOrdinaryTrickChecks`。无需再为这份纯人物配置复制测试。这里引用既有覆盖，**本次没有重新运行它们**。
- 普通图：[700.jpg](https://www.sanguosha.cn/storage/uploads/images/skins/700.jpg)，本次HEAD 200/image/jpeg/224700字节；建议portraitKey `mobile-huang-yueying`，group `standard`。


本地立绘为 `official-mobile-huang-yueying.jpg`，图鉴 `standard`；集成定向验证不表示全量通过。

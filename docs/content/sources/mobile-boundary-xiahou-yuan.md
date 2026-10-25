# 界夏侯渊（移动版）规则来源

核对日期：2026-10-25。魏势力、男性、4 体力，移动版身份场。

- [移动版官网武将页：界夏侯渊](https://www.sanguosha.cn/index.php/pc/hero-detail-309.html)只有神速，包含三种阶段代价。
- [移动版 Wiki：界夏侯渊](https://wiki.biligame.com/msgs/%E7%95%8C%E5%A4%8F%E4%BE%AF%E6%B8%8A)，2026-09-23 更新，身份场完整技能仍只有神速；[神速技能页](https://wiki.biligame.com/msgs/%E7%A5%9E%E9%80%9F/%E7%95%8C%E5%A4%8F%E4%BE%AF%E6%B8%8A)一致。
- [OL 官网：界夏侯渊](https://www.sanguosha.com/hero/453)以及 [2020-05-13 的 OL 上架公告](https://www.sanguosha.com/news/20200513_6040_4217)另有设变。它们属于 OL，不能据此给移动版追加技能。定向检索未发现给移动版界夏侯渊增加设变的后续官方修订。

## 完整技能

**神速**：你可以选择至多三项：1. 跳过判定阶段和摸牌阶段；2. 跳过出牌阶段并弃置一张装备牌；3. 跳过弃牌阶段并翻面。你每选择一项，便视为你使用一张无距离限制的【杀】。

## 实现

移动版 `boundary:xiahou-yuan-mobile` 完整复用 `boundary:shensu` 的三个原生阶段分支，不持有 `boundary:shebian`。第二项可支付手牌或装备区里的真实装备牌；第三项等待翻面引发的子结算后继续原虚拟杀。人物配置之外不新增运行时或重复场景。

既有 `BoundaryXiahouYuanChecks.ThreeShensuBranchesAndHandEquipmentCost` 覆盖三个分支、真实阶段跳过、手牌装备成本及冷恢复。武将及当前牌池注册顺序为 60100，版本为 `boundary-mobile` / `sanguosha-mobile`。

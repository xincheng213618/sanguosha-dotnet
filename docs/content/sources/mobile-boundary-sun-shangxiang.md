# 移动版界孙尚香

核对日期：2026-10-25。独立身份 `boundary:sun-shangxiang-mobile`，吴、女性、三体力，画像键 `boundary-sun-shangxiang-mobile`。

- [移动版官网武将详情236](https://www.sanguosha.cn/index.php/pc/hero-detail-236.html)列出结姻、枭姬；结姻采用每个出牌阶段一次，选择男性角色后弃一张手牌或向其装备区置入一张装备的版本。付款完成后，双方体力较高者摸一、较低者回复一，等体力无这两项收益。
- [移动版官网攻略《飒爽的巾帼女将—界孙尚香》](https://www.sanguosha.cn/pc/guide-info-56.html)，2020-06-16：完整列出两项技能，确认女性、吴、三体力，装备从自己装备区转移可触发枭姬。枭姬是装备区每失去一张牌后可摸两张，并未改为整批只摸两张。攻略中“弃置装备到对方装备区”是口语解释，规则正文采用置入。
- [移动版Wiki界孙尚香](https://wiki.biligame.com/msgs/%E7%95%8C%E5%AD%99%E5%B0%9A%E9%A6%99)，2025-10-21更新：当前完整身份正文与以上两技能一致，结姻明确支付后才比较双方体力。核查时未找到取代上述身份技能的后续官方调整公告。

与现有OL界孙尚香 `boundary:sun-shangxiang` 的两项执行规则没有差异，完整复用 `boundary:jieyin-current` 与 `classic:xiaoji`。不套用标准孙尚香的两手牌共同回复，也不使用SP良助/返乡或谋版使命结姻、护甲及专属枭姬。

结姻两个支付入口共用 `pair-payment` 阶段次数；弃牌限手牌，置入可用手牌或装备区的一张装备。置入是实际区域移动及必要的装备替换，不制造用牌；付款及其枭姬等子结算返回后，原 `ProgramSkillFrame` 冻结一次双方体力，再按该比较摸牌和回复。等体力不发放摸牌或回复，后续体力变化不重新比较。已提交的实体移动、子结算及冷恢复沿既有原帧合同。

本次只登记移动版人物，技能定义、展示及Runtime全部复用，不新增重复行为夹具。既有过滤器 `Equipment placement actual replacement Xiaoji transfer`（`BoundarySunShangxiangChecks.EquipmentAndReplacement`）覆盖真实置入、占位替换、双方收益，以及已付装备转移后真实枭姬窗口的四视图冷恢复；新增人物身份和完整技能组合交给统一内容及图鉴检查。

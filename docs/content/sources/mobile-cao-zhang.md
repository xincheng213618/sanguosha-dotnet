# 移动版曹彰

核对日期：2026-10-25。正式身份 `mobile:cao-zhang`，魏、男性、四体力，版本 `mobile`，规则集 `sanguosha-mobile`，画像键 `mobile-cao-zhang`。

- [移动版官网曹彰111](https://www.sanguosha.cn/index.php/pc/hero-detail-111.html)：读取完整技能HTML；将驰在摸牌阶段选择多摸一张并于本回合禁用、禁打出杀，或少摸一张并于本回合获得杀无距离与额外一次额度。
- [移动版Wiki曹彰](https://wiki.biligame.com/msgs/曹彰)，2025-12-30更新：当前身份正文与官网一致。检索移动版官方后续调整公告，未发现替代这项普通身份规则的公告；不使用其他产品或界限突破的将驰。

完整复用 `classic:jiangchi`。其原生摸牌阶段互斥选择保留不发动的可能；多摸分支使用真实正常摸牌增量，并发出持续到实际回合结束的 use/response 杀禁令，因此决斗等要求打出杀时也受限制。少摸分支减少正常摸牌数，并于同一实际回合增加杀额度、解除杀距离。这里不是移动版界曹彰“出牌阶段开始，摸一张而本阶段只禁使用杀，或弃牌获得阶段增益”的技能。

本次仅新增人物身份配置，不复制 Runtime 或人物夹具。既有 `CaoZhangChecks.DrawMoreBlocksSlashUseAndResponse` 等将驰真实用牌、打出和摸牌政策检查承接能力验证，技能来源和实际回合租约继续由原生机制维护。

独立画像为官网首张[经典形象51300](https://www.sanguosha.cn/storage/uploads/images/skins/51300.jpg)，原始530×750 JPEG，未裁剪或改色，本地 `src/CardGame.Wpf/Assets/official-mobile-cao-zhang.jpg`。

# 韩当（移动版）

> 此版本与 `classic:han-dang` 技能规则等价，独立武将定义已删除。本文保留规则和素材来源；处理名单见 [武将技能去重](../GENERAL_RULES_EQUIVALENCE.md)。

核对日期：2026-10-25。普通移动身份 `mobile:han-dang`，吴、男性、4体力，弓骑、解烦。本人物完全复用已有能力，只增加真实人物版本和官方画像。

- [移动官网武将119](https://www.sanguosha.cn/index.php/pc/hero-detail-119.html)：完整HTML及玩法说明均明确弓骑出牌阶段限一次，弃置一张牌后，本回合攻击范围无限；弃置的是装备牌时，可以再弃置一名其他角色的一张牌。解烦为出牌阶段限定一次，选择一名角色，攻击范围包含该角色的所有角色各选弃一张武器或令其摸一张。
- [官方渠道2021-10-13韩当攻略](https://sanguosha.game.360.cn/211282862/211290418/211370565.html)：技能表同样将弓骑无限攻击范围持续期写为本回合。当前核对未发现有日期的后续官方公告将普通韩当改为阶段结束失效。
- [移动版 BWIKI 韩当](https://wiki.biligame.com/msgs/%E9%9F%A9%E5%BD%93)及[弓骑/韩当](https://wiki.biligame.com/msgs/%E5%BC%93%E9%AA%91/%E9%9F%A9%E5%BD%93)，技能子页更新于2023-01-26：解烦与官网一致，但弓骑时长写为本阶段。这是来源间明确差异；本次遵从官方主源和有日期官方渠道规则，保留本回合，不把Wiki表述直接改作生产规则。

完整引用 `classic:gongqi` 和 `classic:jiefan`。弓骑使用真实HE弃牌、每实际出牌阶段一次、原生本回合攻击范围无限修正；装备牌费用后的他人弃牌为可选，沿既有原生选择和弃牌过程。它只修改攻击范围，不赋予同花色杀无限次数，也不增加普通距离锦囊的使用距离。

解烦使用原生 `RequestAttackRangeAid`，冻结实际攻击范围覆盖名单，再按原生参与者顺序处理：各人可以弃自己手牌或装备区中合法的武器，或者令原目标实际摸一张。没有武器者仍可让原目标摸牌，原目标本人的攻击范围不包括自己。限定配额每局一次，不采用OL界版首轮返还，也不采用移动界版有坐骑直接无限攻击范围、弃非基本牌拆牌，或谋版的响应颜色压制。

纯复用不增加重复人物夹具。既有相关过滤器为 `Mobile boundary Han Dang: frozen responders HE weapons native draw children cold`（同一无返还的原生解烦操作）、`skill executor resumes child resolution without repeating paid effects`，以及已注册的通用主动技能、移动和内容加载检查；图鉴和立绘由本批统一检查。没有新runtime、新pending或新事件。

立绘为[官网原画51000](https://www.sanguosha.cn/storage/uploads/images/skins/51000.jpg)，530×750，原图保存为 `official-mobile-han-dang.jpg`，未裁剪或重采样。SHA-256：`a1b9133f44dd8bf9552c30878ec014fec09edc8a54891698a83708065459c97e`。

# 移动版刘封

> 此版本与 `classic:liu-feng` 技能规则等价，独立武将定义已删除。本文保留规则和素材来源；处理名单见 [武将技能去重](../GENERAL_RULES_EQUIVALENCE.md)。

核对日期：2026-10-25。正式身份 `mobile:liu-feng`，蜀、男性、四体力，版本 `mobile`，规则集 `sanguosha-mobile`，画像键 `mobile-liu-feng`。

- [移动版官网刘封128](https://www.sanguosha.cn/index.php/pc/hero-detail-128.html)：只列陷嗣一项完整身份技能；[官网武将目录](https://www.sanguosha.cn/index.php/pc/hero-list.html)收录标准刘封。
- [移动版Wiki刘封](https://wiki.biligame.com/msgs/刘封)，2026-03-18更新：准备阶段至多两名角色各一张牌成为“逆”；一名角色需要对本人使用杀时，可移去两张逆视为使用普通杀。与官网标准身份规则一致。
- [移动版官方刘封攻略478](https://www.sanguosha.cn/pc/news-detail-478.html)，2018-10-15：同样列出标准陷嗣，并说明可以拿他人或自己的非判定区牌。这是对当前标准版的历史佐证，不作为更晚新技能的替代来源。

完整复用 `classic:xiansi`。准备阶段可以不发动，发动时从至多两名角色的手牌或装备区各取一张，包含自己；同一名角色不能提供两张，不包含判定区。原生公开牌堆收集器冻结实际来源位置及选择槽，真实移动进入本人公开“逆”区后仍保留原技能与付款身份。

其他角色需要对刘封使用杀时，可以从这堆真实“逆”中移去两张，视为对刘封使用普通杀。沿既有 `foreignPublicPileSlash` 的真正移牌付款、原生杀目标、响应、距离及使用规则和父帧返回；不伪造手牌杀，不重付已移去的两张逆，也不在恢复时新选一对替代实体。

本版没有侵凌，没有准备阶段逆数大于体力时本人移去一张逆出杀的效果，也没有 OL 2026 界刘封的同一名角色共给两张牌。正式身份使用移动版标准名称，不将这些其他版本包装进来。本次为完整纯配置复用，不新建重复人物夹具、Runtime、枚举或共享接线；公开牌堆与原生杀的既有机制及冷恢复检查继续适用。

独立画像直接采用官网首张[经典形象17900](https://www.sanguosha.cn/storage/uploads/images/skins/17900.jpg)，原始530×750 JPEG，未裁剪或改色；本地为 `src/CardGame.Wpf/Assets/official-mobile-liu-feng.jpg`。

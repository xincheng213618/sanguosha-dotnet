# 移动版刘虞

> 此版本与 `classic:liu-yu` 技能规则等价，独立武将定义已删除。本文保留规则和素材来源；处理名单见 [武将技能去重](../GENERAL_RULES_EQUIVALENCE.md)。

核对日期：2026-10-26。正式身份 `mobile:liu-yu`，群、男性、基础二体力，人物 `character:liu-yu`，版本 `mobile`、规则集 `sanguosha-mobile`、画像键 `mobile-liu-yu`。移动Wiki归属乱世英杰-2016，图库对应一将成名2016组。

[官网220](https://www.sanguosha.cn/pc/hero-detail-220.html)与[移动Wiki当前刘虞](https://wiki.biligame.com/msgs/刘虞)身份正文一致，Wiki人物固定版本oldid30720。止戈在出牌阶段手牌数大于体力时限一次，选择攻击范围包含自己的其他角色，令其选择使用一张杀或将装备区一张牌交给刘虞；杀的合法目标由被请求者选择。宗祚在游戏开始按势力种类同时增加上限和体力，每个势力最后一名角色死亡后减少一点上限。当前资料没有单独列出已上线新版与此冲突。

完整复用 `classic:zhige`、`classic:zongzuo`。现有止戈的 `chooserRef: selectedTarget` 使被请求者选择真实杀目标和实体，不把杀固定指向刘虞；不使用杀时支付的是装备区实体。宗祚沿原生人口数量表达式与死亡时势力灭绝事实处理，角色初始基础HP仍为2，开局加体力交给技能结算，不预先写入高HP。此为完全等价内容引用，没有新运行时或重复定义快照检查。

正式经典画像为[官网38300](https://www.sanguosha.cn/storage/uploads/images/skins/38300.jpg)，官网220原始HTML首图38300、第二图38301；精确URL的HTTP HEAD为200、image/jpeg。建议本地素材 `src/CardGame.Wpf/Assets/official-mobile-liu-yu.jpg`，由批次统一素材和图库接入。

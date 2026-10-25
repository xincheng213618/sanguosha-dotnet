# 移动版张宝

核对日期：2026-10-26。正式身份 `mobile:zhang-bao`，群、男性、三体力，版本 `mobile`，规则集 `sanguosha-mobile`，画像键 `mobile-zhang-bao`。

[移动版官网张宝81](https://www.sanguosha.cn/pc/hero-detail-81.html)明确咒缚用一张手牌、每回合结束令本回合移除过咒的角色失去体力；影兵按与咒的花色相同判定，第二次摸牌后解除咒。[移动版Wiki张宝](https://wiki.biligame.com/msgs/张宝)（2025-10-21更新）亦按同花色与移去咒表述，但其咒缚结尾仅明确判定使用咒后的失去体力；本项目采用官网的所有移除原因口径，包括第二次影兵移去。

新增独立 `mobile:zhoufu`、`mobile:yingbing`，与允许手牌及装备、按颜色匹配并在二次摸牌后获得咒的OL版本分开。复用原有实体咒、判定替换、原生用牌身份、实际摸牌子流程和回合末失去体力帧。可选 `matchSuit`、`discardAfterSecondDraw` 在定义加载时验证并纳入内容哈希；不提供参数的存量OL规则保留原行为。移动版选择仅发布手牌，实际提交与付款回执按该激活的来源区复核。

第二次真实摸牌的子流程全部返回后，将原咒实体移入弃牌堆，留下独立移除事实；重复恢复沿原帧回执推进，不再次摸牌或移动咒。代表检查使用同色异花色及同花色真实装备使用、在两次已付款摸牌孩子中冷恢复，再验证原回合结束失去体力；另保留OL同颜色并获得原咒的定向回归。

独立画像为官网首张[经典形象11900](https://www.sanguosha.cn/storage/uploads/images/skins/11900.jpg)，资源文件 `src/CardGame.Wpf/Assets/official-mobile-zhang-bao.jpg`；由批次统一绑定和核验。

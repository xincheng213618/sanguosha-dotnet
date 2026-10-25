# 移动版曹节

核对日期：2026-10-26。正式身份 `mobile:cao-jie`，群、女性、三体力，人物 `character:cao-jie`，版本 `mobile`、规则集 `sanguosha-mobile`、画像键 `mobile-cao-jie`。移动Wiki归属乱世英杰-2017，图库对应一将成名2017组。

[官网228](https://www.sanguosha.cn/pc/hero-detail-228.html)与[移动Wiki当前曹节](https://wiki.biligame.com/msgs/曹节)身份版的守玺规则相同，Wiki人物固定版本oldid5600。守玺在成为他人杀的目标后声明本局尚未声明的非装备牌名，使用者弃对应实体并取曹节一张牌，或杀对曹节无效。完整复用 `classic:shouxi` 的有限名称账本、原生目标效果和真实付款流程。

惠民有一处明确文本细化：官网概括为选择一名起始角色，移动Wiki当前正文限定必须选自本次“手牌数小于体力值”的受益者。现有 `classic:huimin` 原生分牌流程已冻结这批受益者，但仍给全体存活者作为起点候选。因此新增 `mobile:huimin`，以现有 `drawAndDraftLowHandPopulation` 的可选 `stateId: eligible-start` 策略限制起点；不直接把经典版当完全等价引用。

策略只在loader允许该精确可选值，省略时保持经典版默认及原有内容指纹形状。正常摸牌、展示、受益者顺序、分牌实体和暂停/返回全部留在原 `PublicHandDraft` 帧上。移动版起点的发布与提交重验都从本次冻结 `RecipientSeats` 筛选仍存活者；不会在惠民摸牌后重新计算资格，也不另设选人sidecar。若冻结受益者均已死亡，原帧完成剩余流程。没有发现已上线的新改版与此规则冲突。

新增一项focused真实命令检查，同时覆盖移动版只提供本次受益者起点、经典默认仍提供全体存活者，以及已摸牌/已展示后的冷恢复和分牌不重复。现有命名防御及公共分牌机制回归继续复用，不逐人物复制。

正式经典画像为[官网39100](https://www.sanguosha.cn/storage/uploads/images/skins/39100.jpg)，官网228原始HTML轮播39100/39101/39102/39103，经典用39100。精确URL的HTTP HEAD为200、image/jpeg。本地素材 `src/CardGame.Wpf/Assets/official-mobile-cao-jie.jpg`，已由批次统一素材和图库接入。

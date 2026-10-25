# 移动版界张昭张纮

角色为 `boundary:zhang-zhao-zhang-hong-mobile`，移动版身份规则，吴、男、3体力。核对日期：2026-10-25。

主源为[移动版官网武将296](https://www.sanguosha.cn/index.php/pc/hero-detail-296.html)，包含完整技能正文、隐藏的重复技能说明及玩法说明；第二来源为[移动版BWIKI界张昭张纮](https://wiki.biligame.com/msgs/%E7%95%8C%E5%BC%A0%E6%98%AD%E5%BC%A0%E7%BA%AE)，页面标注2026-01-11更新。直谏允许将一张手牌装备置入他人空的对应装备槽，不能替换；本人在出牌阶段内正常使用装备也摸一。固政在其他人的弃牌阶段结束时，先归还其该阶段弃置的一张手牌，再自主决定是否获得该阶段其余仍在弃牌堆的弃牌。官网将归还明确限于该角色的手牌，BWIKI补充该阶段全部因弃置进入弃牌堆的牌；以二者共同构成的完整范围实现。检索未发现后续官方移动身份规则修订取代这组正文。

既有OL `boundary:zhijian` 允许装备区作为来源并替换装备；`boundary:guzheng` 是每阶段至少两张牌的即时批次处理，两者均不采用。`classic:zhijian` 的手牌、空槽、真实放置节点直接复用并加入原生移动等待，此外新增本人出牌阶段使用装备的原生摸牌触发。经典固政说明虽相近，实际旧 `RestorePhaseHandDiscards` 只读取手牌上限弃牌并自动获得余牌；另一个旧 `BindDiscardPhaseDiscards` 有八张上限且仅用于结束阶段，因此不能充当当前完整固政。

新固政薄运行时复用实际弃牌阶段令牌、原生逻辑弃牌归属及弃牌实体是否仍可获得的查询。它记录同一原阶段的实际弃牌，原帧冻结全部候选，不限制为八张；归还只能选择阶段主人的实际手牌，余牌可包含该阶段其他真实弃牌。原生 `MoveCard/MoveCards` 负责归还和获得，真实获得、技能变化、体力变化等孩子完成后才询问是否获余牌。选择获得时重新检查原实体仍未离开弃牌堆，无法领取之后新产生或离开后再次进入的同名实体。新pending只存于原 `ProgramSkillFrame`，集合防御性冻结，付款事件只有标量；原归还已经支付后，冷恢复或失去来源仍沿原typed return继续，不能再交还或再次获得。

最少新增Core入口为 `BoundaryZhangZhaoMobileChecks.EmptySlotHandPlacementAndWholeDiscardPhaseReturnClaimCold`：固定四人原生身份桌，验证真实手牌放置后的移动孩子冷恢复、本人正常装备使用摸一、不能替换占用槽；再在他人真实弃牌阶段内同时形成两张手牌弃置和一张装备弃置，归还限手牌，实际归还孩子冷恢复后仍保留可选获得，最终获得另外的手牌和装备各一次。原OL二张检查继续保持其独立规则；共享移动、原生摸牌和冷恢复检查直接复用，不复制整套旧矩阵。

画像取官网296默认第一张[官方48200立绘](https://www.sanguosha.cn/storage/uploads/images/skins/48200.jpg)，原JPEG、530×750，保存为 `src/CardGame.Wpf/Assets/official-boundary-zhang-zhao-zhang-hong-mobile.jpg`，SHA-256为 `957c847ae8e7a01cf2cfd155d1966c4477a8846459dcd8a7bcd7d75a0956f55e`，未裁剪、未重绘。

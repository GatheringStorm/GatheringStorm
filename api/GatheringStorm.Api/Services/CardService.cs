using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GatheringStorm.Api.Data;
using GatheringStorm.Api.Models;
using GatheringStorm.Api.Models.DB;
using GatheringStorm.Api.Models.DB.Effects;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace GatheringStorm.Api.Services
{
    public interface ICardService
    {
        Task<VoidAppResult> InitializeCards();
        Task<VoidAppResult> InitializeGame(Game game);
        Task<AppResult<GameCard>> GenerateStormling(User user);
        Task<VoidAppResult> DrawCard(Game game, string userMail, int cardsCount);
    }

    public class CardService : ICardService
    {
        private readonly AppDbContext dbContext;
        private static readonly Guid StormlingId = Guid.Parse("53746f72-6d6c-696e-6700-000000000000");

        public CardService(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<VoidAppResult> InitializeCards()
        {
            if (this.dbContext.Cards.Any())
            {
                return VoidAppResult.Success();
            }

            var claus = new Character
            {
                Id = Guid.NewGuid(),
                Name = "Claus"
            };
            var sepp = new Character
            {
                Id = Guid.NewGuid(),
                Name = "Sepp"
            };
            var azgul = new Character
            {
                Id = Guid.NewGuid(),
                Name = "Azgul"
            };
            var ison = new Character
            {
                Id = Guid.NewGuid(),
                Name = "Ison"
            };
            var rink = new Character
            {
                Id = Guid.NewGuid(),
                Name = "Rink"
            };
            var aiba = new Character
            {
                Id = Guid.NewGuid(),
                Name = "Aiba"
            };
            var uni = new Character
            {
                Id = Guid.NewGuid(),
                Name = "Uni"
            };
            var okika = new Character
            {
                Id = Guid.NewGuid(),
                Name = "Okika"
            };
            var zorya = new Character
            {
                Id = Guid.NewGuid(),
                Name = "Zorya"
            };
            var yorza = new Character
            {
                Id = Guid.NewGuid(),
                Name = "Yorza"
            };
            var nono = new Character
            {
                Id = Guid.NewGuid(),
                Name = "Nono"
            };
            var yuu = new Character
            {
                Id = Guid.NewGuid(),
                Name = "Yuu"
            };
            var velza = new Character
            {
                Id = Guid.NewGuid(),
                Name = "Velza"
            };
            var palutena = new Character
            {
                Id = Guid.NewGuid(),
                Name = "Palutena"
            };
            var akuga = new Character
            {
                Id = Guid.NewGuid(),
                Name = "Akuga"
            };
            var yerag = new Character
            {
                Id = Guid.NewGuid(),
                Name = "Yerag"
            };
            var drahlget = new Character
            {
                Id = Guid.NewGuid(),
                Name = "Drahlget"
            };
            var stormling = new Character
            {
                Id = Guid.NewGuid(),
                Name = "Stormling"
            };
            await dbContext.Characters.AddRangeAsync(claus, sepp, azgul, ison, rink, aiba, uni, okika,
                zorya, yorza, yuu, velza, palutena, akuga, yerag, drahlget, stormling);

            var pawn = new Title
            {
                Id = Guid.NewGuid(),
                Name = "The pawn"
            };
            var librarian = new Title
            {
                Id = Guid.NewGuid(),
                Name = "The librarian"
            };
            var recruit = new Title
            {
                Id = Guid.NewGuid(),
                Name = "The recruit"
            };
            var berserker = new Title
            {
                Id = Guid.NewGuid(),
                Name = "The berserker"
            };
            var defender = new Title
            {
                Id = Guid.NewGuid(),
                Name = "The defender"
            };
            var soldier = new Title
            {
                Id = Guid.NewGuid(),
                Name = "The soldier"
            };
            var executioner = new Title
            {
                Id = Guid.NewGuid(),
                Name = "The executioner"
            };
            var bard = new Title
            {
                Id = Guid.NewGuid(),
                Name = "The bard"
            };
            var sergeant = new Title
            {
                Id = Guid.NewGuid(),
                Name = "The sergeant"
            };
            var guardian = new Title
            {
                Id = Guid.NewGuid(),
                Name = "The guardian"
            };
            var flameEmpress = new Title
            {
                Id = Guid.NewGuid(),
                Name = "The flame empress"
            };
            var executer = new Title
            {
                Id = Guid.NewGuid(),
                Name = "The executer"
            };
            var demonQueen = new Title
            {
                Id = Guid.NewGuid(),
                Name = "The demon queen"
            };
            var holy = new Title
            {
                Id = Guid.NewGuid(),
                Name = "The holy"
            };
            var sinner = new Title
            {
                Id = Guid.NewGuid(),
                Name = "The sinner"
            };
            var undead = new Title
            {
                Id = Guid.NewGuid(),
                Name = "The undead"
            };
            var monk = new Title
            {
                Id = Guid.NewGuid(),
                Name = "The monk"
            };
            var fist = new Title
            {
                Id = Guid.NewGuid(),
                Name = "The fist"
            };
            var leader = new Title
            {
                Id = Guid.NewGuid(),
                Name = "The leader"
            };
            await dbContext.Titles.AddRangeAsync(pawn, librarian, recruit, berserker, defender,
                soldier, executioner, bard, sergeant, guardian,
                flameEmpress, executer, demonQueen, holy, sinner, undead, monk, fist, leader);

            await dbContext.Cards.AddRangeAsync(new Card
            {
                Id = Guid.NewGuid(),
                Cost = 1,
                Attack = 2,
                BaseHealth = 1,
                IsLegendary = false,
                Title = pawn,
                Character = claus
            },
            new Card
            {
                Id = Guid.NewGuid(),
                Cost = 1,
                Attack = 1,
                BaseHealth = 2,
                IsLegendary = false,
                Title = pawn,
                Character = sepp
            },
            new Card
            {
                Id = Guid.NewGuid(),
                Cost = 1,
                Attack = 0,
                BaseHealth = 2,
                IsLegendary = false,
                Title = librarian,
                Character = azgul,
                Effects = new List<CardEffect>
                {
                    new CardEffect
                    {
                        Id = Guid.NewGuid(),
                        EffectType = EffectType.DrawCards,
                        EffectParameters = JsonConvert.SerializeObject(new DrawCardsEffectParameters
                        {
                            CardsCount = 1
                        })
                    }
                }
            },
            new Card
            {
                Id = Guid.NewGuid(),
                Cost = 2,
                Attack = 2,
                BaseHealth = 3,
                IsLegendary = false,
                Title = recruit,
                Character = ison
            },
            new Card
            {
                Id = Guid.NewGuid(),
                Cost = 2,
                Attack = 3,
                BaseHealth = 2,
                IsLegendary = false,
                Title = recruit,
                Character = rink
            },
            new Card
            {
                Id = Guid.NewGuid(),
                Cost = 2,
                Attack = 1,
                BaseHealth = 4,
                IsLegendary = false,
                Title = recruit,
                Character = aiba
            },
            new Card
            {
                Id = Guid.NewGuid(),
                Cost = 2,
                Attack = 4,
                BaseHealth = 1,
                IsLegendary = false,
                Title = berserker,
                Character = uni
            },
            new Card
            {
                Id = Guid.NewGuid(),
                Cost = 2,
                Attack = 1,
                BaseHealth = 2,
                IsLegendary = false,
                Title = librarian,
                Character = okika,
                Effects = new List<CardEffect>
                {
                    new CardEffect
                    {
                        Id = Guid.NewGuid(),
                        EffectType = EffectType.DrawCards,
                        EffectParameters = JsonConvert.SerializeObject(new DrawCardsEffectParameters
                        {
                            CardsCount = 1
                        })
                    }
                }
            },
            new Card
            {
                Id = Guid.NewGuid(),
                Cost = 2,
                Attack = 1,
                BaseHealth = 3,
                IsLegendary = false,
                Title = defender,
                Character = zorya,
                Effects = new List<CardEffect>
                {
                    new CardEffect
                    {
                        Id = Guid.NewGuid(),
                        EffectType = EffectType.ChangeStats,
                        EffectParameters = JsonConvert.SerializeObject(new ChangeStatsEffectParameters
                        {
                            TargetingType = TargetingType.NumberOfTargets,
                            TargetParameter = "1",
                            EffectStrength = -1
                        })
                    }
                }
            },
            new Card
            {
                Id = Guid.NewGuid(),
                Cost = 3,
                Attack = 3,
                BaseHealth = 4,
                IsLegendary = false,
                Title = soldier,
                Character = sepp
            },
            new Card
            {
                Id = Guid.NewGuid(),
                Cost = 3,
                Attack = 5,
                BaseHealth = 2,
                IsLegendary = false,
                Title = berserker,
                Character = yorza
            },
            new Card
            {
                Id = Guid.NewGuid(),
                Cost = 3,
                Attack = 1,
                BaseHealth = 2,
                IsLegendary = false,
                Title = executioner,
                Character = aiba,
                Effects = new List<CardEffect>
                {
                    new CardEffect
                    {
                        Id = Guid.NewGuid(),
                        EffectType = EffectType.Destroy,
                        EffectParameters = JsonConvert.SerializeObject(new DestroyEffectParameters
                        {
                            TargetingType = TargetingType.NumberOfTargets,
                            TargetParameter = "1"
                        })
                    }
                }
            },
            new Card
            {
                Id = Guid.NewGuid(),
                Cost = 3,
                Attack = 2,
                BaseHealth = 1,
                IsLegendary = false,
                Title = executioner,
                Character = uni,
                Effects = new List<CardEffect>
                {
                    new CardEffect
                    {
                        Id = Guid.NewGuid(),
                        EffectType = EffectType.Destroy,
                        EffectParameters = JsonConvert.SerializeObject(new DestroyEffectParameters
                        {
                            TargetingType = TargetingType.NumberOfTargets,
                            TargetParameter = "1"
                        })
                    }
                }
            },
            new Card
            {
                Id = Guid.NewGuid(),
                Cost = 3,
                Attack = 3,
                BaseHealth = 3,
                IsLegendary = false,
                Title = bard,
                Character = okika,
                Effects = new List<CardEffect>
                {
                    new CardEffect
                    {
                        Id = Guid.NewGuid(),
                        EffectType = EffectType.ChangeStats,
                        EffectParameters = JsonConvert.SerializeObject(new ChangeStatsEffectParameters
                        {
                            TargetingType = TargetingType.NumberOfTargets,
                            TargetParameter = "1",
                            EffectStrength = 1
                        })
                    }
                }
            },
            new Card
            {
                Id = Guid.NewGuid(),
                Cost = 4,
                Attack = 3,
                BaseHealth = 4,
                IsLegendary = false,
                Title = sergeant,
                Character = nono,
                Effects = new List<CardEffect>
                {
                    new CardEffect
                    {
                        Id = Guid.NewGuid(),
                        EffectType = EffectType.ChangeStats,
                        EffectParameters = JsonConvert.SerializeObject(new ChangeStatsEffectParameters
                        {
                            TargetingType = TargetingType.Title,
                            TargetParameter = soldier.Name,
                            EffectStrength = 1
                        })
                    },
                    new CardEffect
                    {
                        Id = Guid.NewGuid(),
                        EffectType = EffectType.ChangeStats,
                        EffectParameters = JsonConvert.SerializeObject(new ChangeStatsEffectParameters
                        {
                            TargetingType = TargetingType.Title,
                            TargetParameter = pawn.Name,
                            EffectStrength = 1
                        })
                    },
                    new CardEffect
                    {
                        Id = Guid.NewGuid(),
                        EffectType = EffectType.ChangeStats,
                        EffectParameters = JsonConvert.SerializeObject(new ChangeStatsEffectParameters
                        {
                            TargetingType = TargetingType.Title,
                            TargetParameter = recruit.Name,
                            EffectStrength = 1
                        })
                    }
                }
            },
            new Card
            {
                Id = Guid.NewGuid(),
                Cost = 4,
                Attack = 3,
                BaseHealth = 4,
                IsLegendary = false,
                Title = bard,
                Character = yuu,
                Effects = new List<CardEffect>
                {
                    new CardEffect
                    {
                        Id = Guid.NewGuid(),
                        EffectType = EffectType.ChangeStats,
                        EffectParameters = JsonConvert.SerializeObject(new ChangeStatsEffectParameters
                        {
                            TargetingType = TargetingType.NumberOfTargets,
                            TargetParameter = "1",
                            EffectStrength = 2
                        })
                    }
                }
            },
            new Card
            {
                Id = Guid.NewGuid(),
                Cost = 4,
                Attack = 3,
                BaseHealth = 2,
                IsLegendary = false,
                Title = executioner,
                Character = velza,
                Effects = new List<CardEffect>
                {
                    new CardEffect
                    {
                        Id = Guid.NewGuid(),
                        EffectType = EffectType.Destroy,
                        EffectParameters = JsonConvert.SerializeObject(new DestroyEffectParameters
                        {
                            TargetingType = TargetingType.NumberOfTargets,
                            TargetParameter = "1"
                        })
                    }
                }
            },
            new Card
            {
                Id = Guid.NewGuid(),
                Cost = 5,
                Attack = 4,
                BaseHealth = 7,
                IsLegendary = false,
                Title = guardian,
                Character = sepp
            },
            new Card
            {
                Id = Guid.NewGuid(),
                Cost = 5,
                Attack = 5,
                BaseHealth = 4,
                IsLegendary = false,
                Title = librarian,
                Character = palutena,
                Effects = new List<CardEffect>
                {
                    new CardEffect
                    {
                        Id = Guid.NewGuid(),
                        EffectType = EffectType.DrawCards,
                        EffectParameters = JsonConvert.SerializeObject(new DrawCardsEffectParameters
                        {
                             CardsCount = 2
                        })
                    }
                }
            },
            new Card
            {
                Id = Guid.NewGuid(),
                Cost = 5,
                Attack = 3,
                BaseHealth = 4,
                IsLegendary = true,
                Title = flameEmpress,
                Character = akuga,
                Effects = new List<CardEffect>
                {
                    new CardEffect
                    {
                        Id = Guid.NewGuid(),
                        EffectType = EffectType.Destroy,
                        EffectParameters = JsonConvert.SerializeObject(new DestroyEffectParameters
                        {
                             TargetingType = TargetingType.NumberOfTargets,
                             TargetParameter = "4"
                        })
                    }
                }
            },
            new Card
            {
                Id = Guid.NewGuid(),
                Cost = 4,
                Attack = 2,
                BaseHealth = 3,
                IsLegendary = true,
                Title = executer,
                Character = akuga,
                Effects = new List<CardEffect>
                {
                    new CardEffect
                    {
                        Id = Guid.NewGuid(),
                        EffectType = EffectType.ChangeStats,
                        EffectParameters = JsonConvert.SerializeObject(new ChangeStatsEffectParameters
                        {
                             TargetingType = TargetingType.CharacterName,
                             TargetParameter = executioner.Name,
                             EffectStrength = 3
                        })
                    }
                }
            },
            new Card
            {
                Id = Guid.NewGuid(),
                Cost = 4,
                Attack = 2,
                BaseHealth = 3,
                IsLegendary = true,
                Title = demonQueen,
                Character = akuga,
                Effects = new List<CardEffect>
                {
                    new CardEffect
                    {
                        Id = Guid.NewGuid(),
                        EffectType = EffectType.ChangeStats,
                        EffectParameters = JsonConvert.SerializeObject(new ChangeStatsEffectParameters
                        {
                             TargetingType = TargetingType.NumberOfTargets,
                             TargetParameter = "1",
                             EffectStrength = 1
                        })
                    },
                    new CardEffect
                    {
                        Id = Guid.NewGuid(),
                        EffectType = EffectType.Destroy,
                        EffectParameters = JsonConvert.SerializeObject(new DestroyEffectParameters
                        {
                             TargetingType = TargetingType.NumberOfTargets,
                             TargetParameter = "1"
                        })
                    },
                    new CardEffect
                    {
                        Id = Guid.NewGuid(),
                        EffectType = EffectType.DrawCards,
                        EffectParameters = JsonConvert.SerializeObject(new DrawCardsEffectParameters
                        {
                             CardsCount = 1
                        })
                    }
                }
            },
            new Card
            {
                Id = Guid.NewGuid(),
                Cost = 3,
                Attack = 1,
                BaseHealth = 4,
                IsLegendary = true,
                Title = holy,
                Character = yerag,
                Effects = new List<CardEffect>
                {
                    new CardEffect
                    {
                        Id = Guid.NewGuid(),
                        EffectType = EffectType.ChangeStats,
                        EffectParameters = JsonConvert.SerializeObject(new ChangeStatsEffectParameters
                        {
                             TargetingType = TargetingType.NumberOfTargets,
                             TargetParameter = "1",
                             EffectStrength = 1
                        })
                    },
                    new CardEffect
                    {
                        Id = Guid.NewGuid(),
                        EffectType = EffectType.DrawCards,
                        EffectParameters = JsonConvert.SerializeObject(new DrawCardsEffectParameters
                        {
                             CardsCount = 2
                        })
                    }
                }
            },
            new Card
            {
                Id = Guid.NewGuid(),
                Cost = 4,
                Attack = 2,
                BaseHealth = 1,
                IsLegendary = true,
                Title = sinner,
                Character = yerag,
                Effects = new List<CardEffect>
                {
                    new CardEffect
                    {
                        Id = Guid.NewGuid(),
                        EffectType = EffectType.Destroy,
                        EffectParameters = JsonConvert.SerializeObject(new DestroyEffectParameters
                        {
                             TargetingType = TargetingType.NumberOfTargets,
                             TargetParameter = "3"
                        })
                    }
                }
            },
            new Card
            {
                Id = Guid.NewGuid(),
                Cost = 3,
                Attack = 1,
                BaseHealth = 3,
                IsLegendary = true,
                Title = undead,
                Character = yerag,
                Effects = new List<CardEffect>
                {
                    new CardEffect
                    {
                        Id = Guid.NewGuid(),
                        EffectType = EffectType.ChangeStats,
                        EffectParameters = JsonConvert.SerializeObject(new ChangeStatsEffectParameters
                        {
                             TargetingType = TargetingType.NumberOfTargets,
                             TargetParameter = "5",
                             EffectStrength = -1
                        })
                    }
                }
            },
            new Card
            {
                Id = Guid.NewGuid(),
                Cost = 3,
                Attack = 3,
                BaseHealth = 2,
                IsLegendary = true,
                Title = monk,
                Character = drahlget,
                Effects = new List<CardEffect>
                {
                    new CardEffect
                    {
                        Id = Guid.NewGuid(),
                        EffectType = EffectType.ChangeStats,
                        EffectParameters = JsonConvert.SerializeObject(new ChangeStatsEffectParameters
                        {
                             TargetingType = TargetingType.CharacterName,
                             TargetParameter = uni.Name,
                             EffectStrength = 1
                        })
                    },
                    new CardEffect
                    {
                        Id = Guid.NewGuid(),
                        EffectType = EffectType.ChangeStats,
                        EffectParameters = JsonConvert.SerializeObject(new ChangeStatsEffectParameters
                        {
                             TargetingType = TargetingType.CharacterName,
                             TargetParameter = aiba.Name,
                             EffectStrength = 1
                        })
                    },
                    new CardEffect
                    {
                        Id = Guid.NewGuid(),
                        EffectType = EffectType.ChangeStats,
                        EffectParameters = JsonConvert.SerializeObject(new ChangeStatsEffectParameters
                        {
                             TargetingType = TargetingType.CharacterName,
                             TargetParameter = yuu.Name,
                             EffectStrength = 1
                        })
                    },
                    new CardEffect
                    {
                        Id = Guid.NewGuid(),
                        EffectType = EffectType.DrawCards,
                        EffectParameters = JsonConvert.SerializeObject(new DrawCardsEffectParameters
                        {
                            CardsCount = 1
                        })
                    }
                }
            },
            new Card
            {
                Id = Guid.NewGuid(),
                Cost = 4,
                Attack = 2,
                BaseHealth = 3,
                IsLegendary = true,
                Title = fist,
                Character = drahlget,
                Effects = new List<CardEffect>
                {
                    new CardEffect
                    {
                        Id = Guid.NewGuid(),
                        EffectType = EffectType.Destroy,
                        EffectParameters = JsonConvert.SerializeObject(new DestroyEffectParameters
                        {
                             TargetingType = TargetingType.Title,
                             TargetParameter = recruit.Name
                        })
                    },
                    new CardEffect
                    {
                        Id = Guid.NewGuid(),
                        EffectType = EffectType.Destroy,
                        EffectParameters = JsonConvert.SerializeObject(new DestroyEffectParameters
                        {
                             TargetingType = TargetingType.Title,
                             TargetParameter = soldier.Name
                        })
                    },
                    new CardEffect
                    {
                        Id = Guid.NewGuid(),
                        EffectType = EffectType.Destroy,
                        EffectParameters = JsonConvert.SerializeObject(new DestroyEffectParameters
                        {
                             TargetingType = TargetingType.Title,
                             TargetParameter = pawn.Name
                        })
                    }
                },
            },
            new Card
            {
                Id = Guid.NewGuid(),
                Cost = 3,
                Attack = 1,
                BaseHealth = 4,
                IsLegendary = true,
                Title = leader,
                Character = drahlget,
                Effects = new List<CardEffect>
                {
                    new CardEffect
                    {
                        Id = Guid.NewGuid(),
                        EffectType = EffectType.ChangeStats,
                        EffectParameters = JsonConvert.SerializeObject(new ChangeStatsEffectParameters
                        {
                             TargetingType = TargetingType.NumberOfTargets,
                             TargetParameter = "1",
                             EffectStrength = 2
                        })
                    },
                    new CardEffect
                    {
                        Id = Guid.NewGuid(),
                        EffectType = EffectType.ChangeStats,
                        EffectParameters = JsonConvert.SerializeObject(new ChangeStatsEffectParameters
                        {
                             TargetingType = TargetingType.Title,
                             TargetParameter = pawn.Name,
                             EffectStrength = -1
                        })
                    },
                    new CardEffect
                    {
                        Id = Guid.NewGuid(),
                        EffectType = EffectType.ChangeStats,
                        EffectParameters = JsonConvert.SerializeObject(new ChangeStatsEffectParameters
                        {
                             TargetingType = TargetingType.Title,
                             TargetParameter = recruit.Name,
                             EffectStrength = -1
                        })
                    },
                }
            },
            new Card
            {
                Id = Guid.NewGuid(),
                Cost = 0,
                Attack = 1,
                BaseHealth = 1,
                IsLegendary = false,
                Character = stormling
            });

            await this.dbContext.SaveChangesAsync();

            return VoidAppResult.Success();
        }

        public async Task<VoidAppResult> InitializeGame(Game game)
        {
            foreach (var participation in game.UserParticipations)
            {
                var cards = new List<GameCard>();
                foreach (var card in this.dbContext.Cards)
                {
                    var cardResult = await this.CreateGameCard(card.Id, participation.User, card.IsLegendary ? 1 : 7);
                    if (cardResult.IsErrorResult)
                    {
                        return cardResult.GetVoidAppResult();
                    }

                    cards.AddRange(cardResult.SuccessReturnValue);
                    game.Entities.AddRange(cardResult.SuccessReturnValue);
                }

                var handCardIndizes = new List<int>();
                var random = new Random();
                while (handCardIndizes.Count < 3)
                {
                    var newIndex = random.Next(cards.Count);
                    if (handCardIndizes.Contains(newIndex))
                    {
                        continue;
                    }
                    cards[newIndex].CardLocation = CardLocation.Hand;
                    handCardIndizes.Add(newIndex);
                }
            }

            return VoidAppResult.Success();
        }

        public async Task<AppResult<GameCard>> GenerateStormling(User user)
        {
            var stormlingCard = await this.dbContext.Cards.SingleOrDefaultAsync(_ => _.Id == StormlingId);
            if (stormlingCard == null)
            {
                return VoidAppResult.Error(ErrorPreset.OnLoadingData).GetErrorAppResult<GameCard>();
            }

            var stormling = new GameCard
            {
                Id = Guid.NewGuid(),
                Health = stormlingCard.BaseHealth,
                Card = stormlingCard,
                CardLocation = CardLocation.Board,
                User = user
            };
            return AppResult<GameCard>.Success(stormling);
        }

        public Task<VoidAppResult> DrawCard(Game game, string userMail, int cardsCount)
        {
            for (var i = 0; i < cardsCount; i++)
            {
                var cards = game.Entities.Select(_ => _ as GameCard)
                                        .Where(_ => _ != null && _.User.Mail == userMail && _.CardLocation == CardLocation.Cellar)
                                        .ToList();

                if (cards.Count == 0)
                {
                    return Task.FromResult(VoidAppResult.Success());
                }

                var drawnCardIndex = new Random().Next(cards.Count());
                cards[drawnCardIndex].CardLocation = CardLocation.Hand;
            }
            return Task.FromResult(VoidAppResult.Success());
        }

        private async Task<AppResult<List<GameCard>>> CreateGameCard(Guid cardId, User user, int duplicatesCount)
        {
            var card = await this.dbContext.Cards.SingleOrDefaultAsync(_ => _.Id == cardId);
            if (card == null)
            {
                return VoidAppResult.Error(ErrorPreset.OnLoadingData).GetErrorAppResult<List<GameCard>>();
            }
            var cards = new List<GameCard>();
            for (var i = 0; i < duplicatesCount; i++)
            {
                cards.Add(new GameCard
                {
                    Id = Guid.NewGuid(),
                    Health = card.BaseHealth,
                    User = user,
                    Card = card,
                    CardLocation = CardLocation.Cellar
                });
            }
            return AppResult<List<GameCard>>.Success(cards);
        }
    }
}
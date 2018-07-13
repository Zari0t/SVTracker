using Newtonsoft.Json;
using System;
using System.Collections.Generic;

using System.IO;

using System.Windows.Forms;

namespace SVTracker
{
    public partial class SVTracker : Form
    {
        string json = "";
        RootObject hash, database = new RootObject();
        Deck deck = new Deck();
        List<Card> cards = new List<Card>();
        int cardsInHand = 0, cardsInDeck = 0, shadowCount = 0;

        public SVTracker()
        {
            InitializeComponent();
        }

        private void SVTracker_Load(object sender, EventArgs e)
        {
            
        }
        private void SVTracker_Shown(object sender, EventArgs e)
        {
            //idk why this works, but this forces the app to check for cards.db being there AFTER opening the main window
            Application.DoEvents();
            Methods.JsonFetch(infoBox, false, 1);
            json = File.ReadAllText(Methods.JsonPathLocal);
        }

        //Kind of what the app is supposed to do
        private void GetDeckButton_Click(object sender, EventArgs e)
        {
            //Deck codes are always 4char long, so...
            if (deckCodeInput.TextLength == 4)
            {
                hash = Methods.GetDeckHash(deckCodeInput.Text);

                //Check if the deck code is valid
                if (hash.Data.Errors.Count == 0)
                {
                    infoBox.AppendText("\r\n\r\nDeck code " + deckCodeInput.Text+" fetched successfully.\r\n");
                    deckBannerList.Controls.Clear();

                    //We're listing cards from local database, so let's load it in here
                    database = JsonConvert.DeserializeObject<RootObject>(json);
                    cards = database.Data.Cards;

                    //Actually fetch the deck's contents, display info regarding it
                    deck = Methods.GetDeck(hash);
                    deckCodeLabel.Text = "Deck Code: "+deckCodeInput.Text;
                    formatLabel.Text = deck.DeckFormatName;

                    Methods.DeckFilter(deck, deckBannerList);

                    cardsInHand = 0;
                    shadowCount = 0;
                    if (deck.DeckFormat == 1)
                        cardsInDeck = 40;
                    else cardsInDeck = 30;
                    numberInHandLabel.Text = "Cards in hand: " + cardsInHand;
                    numberInDeckLabel.Text = "Cards in deck: " + cardsInDeck;
                    shadowCountLabel.Text = "Shadows: " + shadowCount;
                    numberInHandLabel.Show();
                    numberInDeckLabel.Show();
                    shadowCountLabel.Show();
                    ResonanceCheck();
                }

                //Lists error code and message if the deck code was invalid for some reason
                else
                {
                    infoBox.AppendText("\r\n\r\n"+hash.Data.Errors[0].ErrorType);
                    infoBox.AppendText("\r\n"+hash.Data.Errors[0].ErrorMessage);
                }
            }

            // :V
            else
                infoBox.AppendText("\r\n\r\nDeck code must be 4 characters long.");
            deckCodeInput.SelectAll();
        }

        private void ForceFetchEnJsonButton_Click(object sender, EventArgs e)
        {
            json = Methods.JsonFetch(infoBox, true, 1);
        }

        private void ForceFetchJpJsonButton_Click(object sender, EventArgs e)
        {
            json = Methods.JsonFetch(infoBox, true, 2);
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //Resets everything to "game start"
        private void ResetButton_Click(object sender, EventArgs e)
        {
            cardsInHand = 0;
            shadowCount = 0;
            if (deck.DeckFormat == 1)
                cardsInDeck = 40;
            else cardsInDeck = 30;
            numberInHandLabel.Text = "Cards in hand: " + cardsInHand;
            numberInDeckLabel.Text = "Cards in deck: " + cardsInDeck;
            shadowCountLabel.Text = "Shadows: " + shadowCount;
            handBannerList.Controls.Clear();
            ResonanceCheck();
            Methods.DeckFilter (deck, deckBannerList);
        }

        private void SVTracker_Activated(object sender, EventArgs e)
        {
            deckCodeInput.Focus();
            deckCodeInput.SelectAll();
        }

        //Checks if Resonance is active or not
        public void ResonanceCheck()
        {
            if (cardsInDeck % 2 == 0)
                resonanceLabel.Show();
            else resonanceLabel.Hide();
        }

        //Adds a card to the player's hand
        public void AddToHand(int targetId, bool isDraw)
        {
            //Create instance of card to add
            Card targetCard = cards.Find(x => x.CardId == targetId);

            //Check hand size
            if (cardsInHand < 9)
            {
                CardBanner banner = new CardBanner(targetCard.CardId, targetCard.CardName, targetCard.Cost, targetCard.RarityId, 1, false);

                //Each card is a separate banner, even if a player draws into multiples
                banner.countLabel.ResetText();

                //Put newly created CardBanner in hand
                handBannerList.Controls.Add(banner);
                if (isDraw)
                    infoBox.AppendText("\r\nDrew " + targetCard.CardName + ".");

                //Update numbers I guess
                cardsInHand++;
                numberInHandLabel.Text = "Cards in hand: " + cardsInHand;
            }
            //If hand was too full...
            else
            {
                shadowCount++;
                infoBox.AppendText("\r\nHand is too full! Overdrew " + targetCard.CardName + ".");
                shadowCountLabel.Text = "Shadows: " + shadowCount;
            }

            if (isDraw)
            {
                cardsInDeck--;
                numberInDeckLabel.Text = "Cards in deck: " + cardsInDeck;
                ResonanceCheck();
            }
        }

        //Adds a card to the player's deck
        public void AddToDeck(int targetId)
        {
            //control variable, it makes sense dw
            bool notInDeck = true;

            //Create instance of the card we're adding
            Card targetCard = cards.Find(x => x.CardId == targetId);

            //Check to see if the card is already in the deck
            //If it is, simply increase its count by 1
            foreach (CardBanner control in deckBannerList.Controls)
            {
                if (control.cardId == targetCard.CardId)
                {
                    control.cardCount++;
                    control.countLabel.Text = "×" + control.cardCount;
                    notInDeck = false;
                }
            }
            //If it isn't, add a new banner for it
            if (notInDeck)
            {
                CardBanner banner = new CardBanner(targetCard.CardId, targetCard.CardName, targetCard.Cost, targetCard.RarityId, 1, true);
                deckBannerList.Controls.Add(banner);
            }

            //Update numbers I guess
            cardsInDeck++;
            numberInDeckLabel.Text = "Cards in deck: " + cardsInDeck;
            ResonanceCheck();
        }

        //-------------------------------------------------------------------

        //This is a horrible idea.

        //Check which card was played and...do things accordingly
        public void PlayCard(int sourceId)
        {
            //Create instance of the card being played
            cardsInHand--;
            shadowCount++;
            Card sourceCard = cards.Find(x => x.CardId == sourceId);
            if (sourceCard != null)
                infoBox.AppendText("\r\nPlayed " + sourceCard.CardName + ". ");

            //oh boy here we go
            switch (sourceCard.CardId)
            {
                //Neutral
                case 106024010:                         //Mystic Ring
                                                        //targetInHand.AddToDeck();
                    break;
                case 103021040:                         //Gourmet Emperor, Khaiza
                    AddToHand(900021010, false);
                    infoBox.AppendText("Added an Ultimate Carrot to hand.");
                    break;
                case 109011020:                         //Lowain of the Brofamily
                    AddToHand(900011050, false);
                    infoBox.AppendText("Added an Elsam of the Brofamily to hand.");
                    break;
                case 900011050:                         //Elsam of the Brofamily
                    AddToHand(900011060, false);
                    infoBox.AppendText("Added a Tomoi of the Brofamily to hand.");
                    break;
                case 900011060:                         //Tomoi of the Brofamily
                    AddToHand(900011070, false);
                    infoBox.AppendText("Added a Human! Pyramid! Attack! to hand.");
                    break;
                case 101031030:                         //Angel Crusher
                case 104434010:                         //Dragonslayer's Price (not Neutral!)
                case 103441010:                         //Imperial Dragoon (not Neutral!)
                    shadowCount += handBannerList.Controls.Count;
                    handBannerList.Controls.Clear();
                    infoBox.AppendText("Discarded all cards in hand.");
                    break;
                case 101041030:                         //Prince of Darkness
                    HolyFuckItsStan();
                    break;

                //Forest
                case 101114050:                         //Fairy Circle
                case 100111020:                         //Fairy Whisperer
                case 106124010:                         //Elf Song
                case 105123010:                         //Wood of Brambles
                case 105111010:                         //Flower Princess
                case 107111020:                         //Weald Philosopher
                case 102121040:                         //Baalt, King of the Elves
                    AddFairy(2);
                    break;
                case 100114010:                         //Sylvan Justice
                case 101121010:                         //Waltzing Fairy
                case 103111030:                         //Fairy Knight
                case 108111010:                         //Elf General
                    AddFairy(1);
                    break;
                case 101111080:                         //Fairy Caster
                    AddFairy(3);
                    break;
                case 108131030:                         //Storied Falconer
                case 103134010:                         //Selwyn's Command
                    AddToHand(900131010, false);
                    infoBox.AppendText("Added a Skystride Raptor to hand.");
                    break;
                case 106121020:                         //Ariana, Natural Tutor
                    AddToHand(104114010, false);
                    infoBox.AppendText("Added an Ivy Spellbomb to hand.");
                    break;
                case 107141010:                         //Aria, Guiding Fairy
                    AddToHand(900111020, false);
                    infoBox.AppendText("Added a Fairy Wisp to hand.");
                    break;
                case 106131010:                         //Venus
                    AddToHand(101122010, false);
                    infoBox.AppendText("Added a Harvest Festival to hand.");
                    break;
                case 101141010:                         //Fairy Princess
                    AddFairy(9 - cardsInHand);
                    break;
                case 101141030:                         //Rose Queen
                    foreach (CardBanner cardBanner in handBannerList.Controls)
                    {
                        if (cardBanner.cardId == 900111010)
                        {
                            cardBanner.Dispose();
                            AddToHand(900144010, false);
                        }
                    }
                    infoBox.AppendText("Transformed all Fairies in hand into Thorn Bursts.");
                    break;

                //Sword
                case 109214010:                         //Grand Auction
                    AddToHand(109223010, false);
                    infoBox.AppendText("Added an Ageworn Weaponry to hand.");
                    break;
                case 103231020:                         //Amelia, Silver Paladin
                    AddToHand(103221030, false);
                    infoBox.AppendText("Added a Gelt, Vice Captain to hand.");
                    break;

                //Rune
                case 103321030:                         //Dwarf Alchemist
                    AddToHand(900312010, false);
                    infoBox.AppendText("Added an Earth Essence to hand.");
                    break;
                case 105334010:                         //Golem Assault
                    AddToHand(900314010, false);
                    infoBox.AppendText("Added a Conjure Guardian to hand.");
                    break;
                case 101334030:                         //First Curse
                    AddToHand(900334010, false);
                    infoBox.AppendText("Added a Second Curse to hand.");
                    break;
                case 107314020:                         //Chain Lightning
                    if (cardsInHand <= 4)
                    {
                        AddToHand(107314020, false);
                        infoBox.AppendText("Added a Chain Lightning to hand.");
                    }
                    break;
                case 103321020:                         //Disaster Witch
                    AddToHand(900314020, false);
                    infoBox.AppendText("Added a Crimson Sorcery to hand.");
                    break;
                case 900321010:                         //Mysterian Whitewyrm
                    AddToHand(900314050, false);
                    infoBox.AppendText("Added a Mysterian Circle to hand.");
                    break;
                case 900321020:                         //Mysterian Blackwyrm
                    AddToHand(900314040, false);
                    infoBox.AppendText("Added a Mysterian Missile to hand.");
                    break;
                case 900334010:                         //Second Curse
                    AddToHand(900334020, false);
                    infoBox.AppendText("Added a Final Curse to hand.");
                    break;
                case 103341010:                         //Daria, Dimensional Witch
                    handBannerList.Controls.Clear();
                    infoBox.AppendText("Banished all cards in hand.");
                    break;

                //Dragon
                case 108441030:                         //Whitefrost Dragon, Filene
                    AddToHand(900444010, false);
                    infoBox.AppendText("Added a Whitefrost Whisper to hand.");
                    break;
                case 109431020:                         //Pyrewyrm Commander
                    AddToHand(100414020, false);
                    infoBox.AppendText("Added a Blazing Breath to hand.");
                    break;
                case 106441020:                         //Python
                    foreach (CardBanner cardBanner in deckBannerList.Controls)
                    {
                        if (cardBanner.cardCost <= 7)
                        {
                            cardBanner.Dispose();
                        }
                    }
                    infoBox.AppendText("Banished all 7pp or less cost cards from deck.");
                    break;
                case 109441010:                         //Zooey, Arbiter of the Skies
                    AddToDeck(900441030);
                    infoBox.AppendText("Added a 10pp 6/5 Zooey, Arbiter of the Skies to the deck.");
                    break;

                //Shadow
                case 101521010:                         //Skeleton Fighter
                    Necromancy(1);
                    break;
                case 100514010:                         //Undying Resentment
                case 103511060:                         //Zombie Buccaneer
                    Necromancy(2);
                    break;
                case 108511030:                         //Mischievous Spirit
                    Necromancy(3);
                    break;
                case 101511010:                         //Skeleton Viper
                case 101511110:                         //Lesser Mummy
                case 101524020:                         //Ethereal Form
                case 100511030:                         //Apprentice Necromancer
                case 101531010:                         //Wight King
                case 101521080:                         //Soulhungry Wraith
                case 102511040:                         //Commander of Destruction
                case 100514020:                         //Call of the Void
                case 101511070:                         //Cursed Soldier
                case 101511100:                         //Orcus
                case 102521020:                         //Andras
                    Necromancy(4);
                    break;
                case 900521030:                         //Fran's Attendant
                case 101521020:                         //Deadly Widow
                case 105521010:                         //Usher of Styx
                case 101534020:                         //Foul Tempest
                case 101534030:                         //Death's Breath
                case 109541020:                         //Mordecai, Eternal Duelist
                    Necromancy(6);
                    break;
                case 107541010:                         //Underworld Ruler Aisha
                    Necromancy(10);
                    break;
                case 101531040:                         //Deathly Tyrant
                    Necromancy(20);
                    break;
                case 100511010:                         //Spartoi Sergeant
                case 101514030:                         //Soul Hunt
                    shadowCount++;
                    infoBox.AppendText("Gained 1 shadow.");
                    break;
                case 100511040:                         //Elder Spartoi Soldier
                    shadowCount++;
                    shadowCount++;
                    infoBox.AppendText("Gained 2 shadows.");
                    break;
                case 101541010:                         //Cerberus
                    AddToHand(900544010, false);
                    AddToHand(900544020, false);
                    break;
                case 108541020:                         //Hinterland Ghoul
                    PtpWithLegs();
                    break;

                //Blood
                case 108641030:                         //Waltz, King of Wolves
                    AddToHand(104633010, false);
                    infoBox.AppendText("Added a Blood Moon to hand.");
                    break;
                case 107641020:                         //Diabolus Psema
                    AddToHand(101014030, false);
                    AddToHand(100624010, false);
                    infoBox.AppendText("Added a Demonic Strike and a Demonic Storm to hand.");
                    break;

                //Haven
                case 108722010:                         //Temple of the Holy Lion
                case 108721010:                         //Peaceweaver
                case 109724010:                         //Prism Swing
                    AddToHand(108722010, false);
                    infoBox.AppendText("Added a Holy Lion Crystal to hand.");
                    break;
                case 105711020:                         //Monk of Purification
                    AddToHand(102714040, false);
                    AddToHand(102714040, false);
                    infoBox.AppendText("Added two Monastic Holy Waters to hand.");
                    break;

                //Portal
                case 107824020:                         //Metaproduction
                    AddToDeck(900811030);
                    infoBox.AppendText("Added an Analyzing Artifact to the deck.");
                    break;
                case 107823010:                         //Ancient Amplifier
                case 107811100:                         //Mech Wing Swordsman
                case 107811030:                         //Gravikinetic Warrior
                                                        //two at random 900811010 900811020 900811030 900811040
                    break;
                case 109813010:                         //Ancient Apparatus
                                                        //two at random 900811020 900811040 900811060
                    break;
                case 100811070:                         //Magisteel Lion
                    AddToDeck(900811030);
                    AddToDeck(900811030);
                    infoBox.AppendText("Added two Analyzing Artifacts to the deck.");
                    break;
                case 107821030:                         //Icarus
                case 107811050:                         //Cat Cannoneer
                    AddToDeck(900811010);
                    AddToDeck(900811010);
                    infoBox.AppendText("Added two Ancient Artifacts to the deck.");
                    break;
                case 108811010:                         //Knower of History
                    AddToDeck(900811060);
                    infoBox.AppendText("Added a Prime Artifact to the deck.");
                    break;
                case 900821010:                         //L'Ange Miriam
                    AddToDeck(900811040);
                    AddToDeck(900811060);
                    infoBox.AppendText("Added a Radiant Artifact and a Prime Artifact to the deck.");
                    break;
                case 100811010:                         //Toy Soldier
                case 108811030:                         //Junk
                case 107824010:                         //Substitution
                case 107813020:                         //Puppet Room
                case 107811120:                         //Masked Puppet
                case 107821100:                         //Automaton Soldier
                case 108834010:                         //Heartless Battle
                case 107821060:                         //Vengefull Puppeteer Noah
                    AddPuppet(1);
                    break;
                case 100824010:                         //Puppeteer's Strings
                    AddPuppet(2);
                    break;
                case 108841010:                         //Orchis, Puppet Girl
                    AddPuppet(3);
                    break;
                case 107811070:                         //Iron Staff Mechanic
                    AddToDeck(900811020);
                    AddToDeck(900811020);
                    infoBox.AppendText("Added two Mystic Artifacts to the deck.");
                    break;
                case 100811040:                         //Ironforged Fighter
                    AddToDeck(900811040);
                    AddToDeck(900811040);
                    infoBox.AppendText("Added two Radiant Artifacts to the deck.");
                    break;
                case 109811030:                         //Enkidu
                    if (cardsInDeck % 2 == 0)
                    {
                        AddToHand(101021030, false);
                        infoBox.AppendText("\r\nResonance effect activated. Added a Gilgamesh to hand.");
                    }
                    break;

                //Literally everything else - I THINK I'm not missing anything(?)
                //Manually maintaining this is going to be a pain in the ass and I regret it already
                default:
                    break;
            }

            //Update labels
            numberInHandLabel.Text = "Cards in hand: " + cardsInHand;
            shadowCountLabel.Text = "Shadows: " + shadowCount;
            ResonanceCheck();
        }

        //PoD aka Stan aka Satan
        public void HolyFuckItsStan()
        {
            //Delete deck contents
            deckBannerList.Controls.Clear();
            AddToDeck(900041010);                       //3x Servant of Darkness
            AddToDeck(900041010);
            AddToDeck(900041010);
            AddToDeck(900041020);                       //3x Silent Rider
            AddToDeck(900041020);
            AddToDeck(900041020);
            AddToDeck(900044010);                       //3x Dis's Damnation
            AddToDeck(900044010);
            AddToDeck(900044010);
            AddToDeck(900044020);                       //1x Astaroth's Reckoning
            infoBox.AppendText("Replaced deck with Apocalypse deck.");
            cardsInDeck = 10;
            numberInDeckLabel.Text = "Cards in deck: 10";
            //Here's to hoping this is the only special case I ever have to code in
        }

        //it wasn't
        public void PtpWithLegs()
        {
            for (int i = 0; i < 10; i++)
                    AddToHand(900511010, false);
            infoBox.AppendText("Added 10 Skeletons to hand.");
        }

        //Auxiliary stuff (because fuck writing the same thing repeatedly)
        public void AddFairy(int count)
        {
            for (int i=0; i<count; i++)
                AddToHand(900111010, false);
            if (count == 1)
                infoBox.AppendText("Added a Fairy to hand.");
            else infoBox.AppendText("Added "+ count + " Fairies to hand.");
        }

        public void AddPuppet(int count)
        {
            for (int i = 0; i < count; i++)
                AddToHand(900811050, false);
            if (count == 1)
                infoBox.AppendText("Added a Puppet to hand.");
            else infoBox.AppendText("Added " + count + " Puppets to hand.");
        }

        public void Necromancy(int cost)
        {
            if (shadowCount >= cost)
            {
                shadowCount -= cost;
                infoBox.AppendText("Necromancy (" + cost + ") activated.");
            }
        }
    }
}

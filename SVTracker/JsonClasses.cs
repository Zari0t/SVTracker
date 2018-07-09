using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVTracker
{
    public class DataHeaders
    {
        [JsonProperty("udid")]
        public bool Udid;

        [JsonProperty("viewer_id")]
        public int ViewerId;

        [JsonProperty("sid")]
        public string Sid;

        [JsonProperty("servertime")]
        public int ServerTime;

        [JsonProperty("result_code")]
        public int ResultCode;
    }

    public class Card
    {
        //SVAPI card structure

        [JsonProperty("card_id")]
        public int CardId;                              //Internal ID number of the card

        [JsonProperty("card_set_id")]
        public int CardSetId;                           //Internal ID number of the card the set is from

        [JsonProperty("card_name")]
        public string CardName;                         //The card's name

        [JsonProperty("char_type")]
        public int TypeId;                              //The card's type, as int (1-4)

        [JsonProperty("clan")]
        public int CraftId;                             //The craft the card belongs to, as int (0-8)

        [JsonProperty("tribe_name")]
        public string Trait;                            //The card's trait ("-" if it doesn't have one)

        [JsonProperty("skill_disc")]
        public string SkillDisc;                        //Effect/ability text of the base form of the card, without formatting

        [JsonProperty("org_skill_disc")]
        public string OrgSkillDisc;                     //Effect/ability text of the base form of the card, with BBSCode formatting on keywords and child cards

        [JsonProperty("evo_skill_disc")]
        public string EvoSkillDisc;                     //Effect/ability text of the evolved form of the card (if it's a follower), without formatting

        [JsonProperty("org_evo_skill_disc")]
        public string OrgEvoSKillDisc;                  //Effect/ability text of the evolved form of the card (if it's a follower), with BBSCode formatting on keywords and child cards

        [JsonProperty("cost")]
        public int Cost;                                //The card's PP cost

        [JsonProperty("atk")]
        public int Attack;                              //The attack value of the base form of the card (if it's a follower)

        [JsonProperty("life")]
        public int Defense;                             //The defense value of the base form of the card (if it's a follower)

        [JsonProperty("evo_atk")]
        public int EvoAttack;                           //The attack value of the evolved form of the card (if it's a follower)

        [JsonProperty("evo_life")]
        public int EvoDefense;                          //The attack value of the evolved form of the card (if it's a follower)

        [JsonProperty("rarity")]
        public int RarityId;                            //The card's rarity, as int (1-4)

        [JsonProperty("description")]
        public string FlavourText;                      //Flavour text of the base form of the card

        [JsonProperty("evo_description")]
        public string EvoFlavourText;                   //Flavour text of the evolved form of the card (if it's a follower)

        [JsonProperty("base_card_id")]
        public int BaseCardId;                          //Internal ID number of the original card; will only be different from card_id if the card is a reprint or an alternate art version

        [JsonProperty("format_type")]
        public bool FormatType;                         //Whether or not the card is Rotation-legal (true if it is)

        [JsonProperty("restricted_count")]
        public int RestrictedCount;                     //How many copies of the card can be used in an Unlimited deck


        //Added fields for ease of use
        [JsonProperty("char_type_name")]
        public string TypeName;                         //The card's type, as string (Follower, Amulet, Spell)

        [JsonProperty("clan_name")]
        public string CraftName;                        //The name of the craft the card belongs to (Neutral, Forestcraft, Swordcraft...)

        [JsonProperty("rarity_name")]
        public string RarityName;                       //The card's rarity, as string (Bronze, Silver, Gold, Legendary)


        //Unused and/or currently unnecessary fields present in the API, commented out in case they're needed at a later point in time
        /*
         * public int foil_card_id;                     //Internal ID number of the animated version of the card; is always card_id+1
         * public bool is_foil;                         //Unused; used internally in the game to check if a card is animated or not
         * public string skill;                         //Unused; used internally in the game to note which kinds of abilities a card has
         * public string skill_option;                  //Unused; used internally in the game to pass arguments to card abilities that require them
         * public int get_red_ether;                    //How many red vials you obtain when disenchanting the card
         * public int use_red_ether;                    //How many red vials you need to craft the card
         * public string cv;                            //The name of the voice actor of the card (if it has one). Only present in JA json
         * public string copyright;                     //Copyright information for special crossover event cards (F/GO cards)
         * public int tokens;                           //Unknown
         * public int normal_card_id;                   //Unknown; seems to be a duplicate of card_id
         */
    }

    public class Error
    {

        [JsonProperty("type")]
        public string ErrorType;

        [JsonProperty("message")]
        public string ErrorMessage;

    }

    public class Data
    {

        [JsonProperty("text")]
        public string Text;

        [JsonProperty("hash")]
        public string DeckHash;

        [JsonProperty("deck_format")]
        public int DeckFormat;

        [JsonProperty("deck")]
        public Deck Deck;

        [JsonProperty("cards")]
        public List<Card> Cards;

        [JsonProperty("errors")]
        public List<Error> Errors;

        //public int clan;

    }

    public class Deck
    {
        [JsonProperty("deck_format")]
        public int DeckFormat;

        [JsonProperty("deck_format_name")]
        public string DeckFormatName;

        [JsonProperty("clan")]
        public int Craft;

        [JsonProperty("cards")]
        public List<Card> Cards;

    }

    public class RootObject
    {

        [JsonProperty("data_headers")]
        public DataHeaders DataHeaders;

        [JsonProperty("data")]
        public Data Data;

    }
}


/* Because it gets confusing:
 * 
 * all .json files
 *  - data_headers (5 items, all unused)
 *    > udid(bool)          - Unknown; seems to always return false
 *    > viewer_id(int)      - Unknown; seems to always return 0
 *    > sid(string)         - Unknown; seems to always return ""
 *    > servertime(int)     - Timestamp of access
 *    > result_code(int)    - Unkonwn; seems to always return 1
 *  - data
 *    > errors (2 items)    - Returns null if no errors occur generating the .json, otherwise...
 *      » type(string)      - Type of error
 *      » message(string)   - Description of the error
 *    
 * cards.json
 *  - data_headers
 *  - data (2 items)
 *    > cards(List<Card>)   - List of all cards in the game and all their attributes
 *    > errors
 *    
 * deckCode.json
 *  - data_headers
 *  - data (4 items)
 *    > text(string)        - Always returns "デッキのインポートに成功しました。"
 *    > clan(int)           - Craft the deck belongs to (1-8)
 *    > hash(string)        - Deck hash, used for fetching the deck itself with deck.json
 *    > errors
 *    
 * deck.json
 *  - data_headers
 *  - data (2 items)
 *    > deck (3 items)
 *      » deck_format(int)  - Returns 1 if Constructed, 2 if TakeTwo
 *      » clan(int)         - Craft the deck belongs to (1-8)
 *      » cards(List<Card>) - List of cards in deck
 *    > errors
 */
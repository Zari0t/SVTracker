using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVTracker
{
    public class DataHeaders
    {
        public bool udid { get; set; }
        public int viewer_id { get; set; }
        public string sid { get; set; }
        public int servertime { get; set; }
        public int result_code { get; set; }
    }

    public class Card
    {
        //SVAPI card structure
        public int card_id { get; set; }                //Internal ID number of the card
        public int card_set_id { get; set; }            //Internal ID number of the card the set is from
        public string card_name { get; set; }           //The card's name
        public int char_type { get; set; }              //The card's type, as int (1-4)
        public int clan { get; set; }                   //The craft the card belongs to, as int (0-8)
        public string tribe_name { get; set; }          //The card's trait ("-" if it doesn't have one)
        public string skill_disc { get; set; }          //Effect/ability text of the base form of the card, without formatting
        public string org_skill_disc { get; set; }      //Effect/ability text of the base form of the card, with BBSCode formatting on keywords and child cards
        public string evo_skill_disc { get; set; }      //Effect/ability text of the evolved form of the card (if it's a follower), without formatting
        public string org_evo_skill_disc { get; set; }  //Effect/ability text of the evolved form of the card (if it's a follower), with BBSCode formatting on keywords and child cards
        public int cost { get; set; }                   //The card's PP cost
        public int atk { get; set; }                    //The attack value of the base form of the card (if it's a follower)
        public int life { get; set; }                   //The defense value of the base form of the card (if it's a follower)
        public int evo_atk { get; set; }                //The attack value of the evolved form of the card (if it's a follower)
        public int evo_life { get; set; }               //The attack value of the evolved form of the card (if it's a follower)
        public int rarity { get; set; }                 //The card's rarity, as int (1-4)
        public string description { get; set; }         //Flavour text of the base form of the card
        public string evo_description { get; set; }     //Flavour text of the evolved form of the card (if it's a follower)
        public int base_card_id { get; set; }           //Internal ID number of the original card; will only be different from card_id if the card is a reprint or an alternate art version
        public bool format_type { get; set; }           //Whether or not the card is Rotation-legal (true if it is)
        public int restricted_count { get; set; }       //How many copies of the card can be used in an Unlimited deck

        //Added fields for ease of use
        public string char_type_name { get; set; }      //The card's type, as string (Follower, Amulet, Spell)
        public string clan_name { get; set; }           //The name of the craft the card belongs to (Neutral, Forestcraft, Swordcraft...)
        public string rarity_name { get; set; }         //The card's rarity, as string (Bronze, Silver, Gold, Legendary)

        //Unused and/or currently unnecessary fields present in the API, commented out in case they're needed at a later point in time
        /*
         * public int foil_card_id { get; set; }        //Internal ID number of the animated version of the card; is always card_id+1
         * public bool is_foil { get; set; }            //Unused; used internally in the game to check if a card is animated or not
         * public string skill { get; set; }            //Unused; used internally in the game to note which kinds of abilities a card has
         * public string skill_option { get; set; }     //Unused; used internally in the game to pass arguments to card abilities that require them
         * public int get_red_ether { get; set; }       //How many red vials you obtain when disenchanting the card
         * public int use_red_ether { get; set; }       //How many red vials you need to craft the card
         * public string cv { get; set; }               //The name of the voice actor of the card (if it has one). Only present in JA json
         * public string copyright { get; set; }        //Copyright information for special crossover event cards (F/GO cards)
         * public int tokens { get; set; }              //Unknown
         * public int normal_card_id { get; set; }      //Unknown; seems to be a duplicate of card_id
         */
    }

    public class Error
    {
        public string type { get; set; }
        public string message { get; set; }
    }

    public class Data
    {
        public string text { get; set; }
        //public int clan { get; set; }
        public string hash { get; set; }
        public int deck_format { get; set; }
        public Deck deck { get; set; }
        public List<Card> cards { get; set; }
        public List<Error> errors { get; set; }
    }

    public class Deck
    {
        public int deck_format { get; set; }
        public int clan { get; set; }
        public List<Card> cards { get; set; }
    }

    public class RootObject
    {
        public DataHeaders data_headers { get; set; }
        public Data data { get; set; }
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
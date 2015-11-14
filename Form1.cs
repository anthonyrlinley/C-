/*
 * Anthony Linley
 * Pipelined MIPS processor simulator
 * 
 * GUI version of old Computer Organization course final project.
 * User must enter 6 (may be less than 6 but no  more than 6) hexadecimal instructions
 * and have each instruction executed through instruction cycle.
 * 
 * Originally coded in Python for course final project. 
 * Recoded in C++ for refresher in language.
 * Currently written in C#/.NET to put knowledge of language into practice
 * 
 * As of 11/14/15 -----
 * Currently meets all of the original project requirements.
 * 
 * Will be adding more functionality soon.
 * Will also be updating/making code more effiecient/more readable soon.
 * 
 * Will probably go back and use built-in conversion functions
 * Originally used user-defined functions as project requirement
 * 
 * **/

using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MIPS_ISA_GUI
{
    public partial class Form1 : Form
    {
        //creates List called hexInstructions with capacity of 6. 
        //holds strings with hexadecimal instructions to be executed
        List<string> hexInstructions = new List<string>(6);
        
        //creates List called binInstructions with capacity of 6.
        //holds strings with converted binary values of hex instructions.
        List<string> binInstructions = new List<string>(6);

        //used to check termination condition for do...while() loop in run_btn() function
        int i = 0;
        
        //used to terminate do...while() loop in run_btn() function
        int j = 5;

        //keeps track of how many instructions have been input
        static int instruction_count = 0;

        //starts program counter at 262144
        //converts to 00040000 in hexadecimal
        //conversion takes place in instruction_fetch() function
        //used to simulate instruction address.
        //will add to jump/branch instructions in future.
        static int program_counter = 262144;

        //clock cycle counter variable
        static int clock_cycle = 0;
        
        //IF/ID pipeline register to hold instruction
        //in between instruction fetch and instruction decode stages
        public string[] if_id_pipeline = new string[6];

        //ID/EX pipeline register to hold information
        //in between instruction fetch and execute stages

        //did not use Lists because kept receiving ArgumentOutOfRange Exception
        //used multiple arrays instead of struct because values would pass from function to function

        //holds register numbers not values in registers
        public string[] I_E_pipeline_instruction = new string[6];
        public int[] I_E_pipeline_RS= new int[6];
        public int[] I_E_pipeline_RT = new int[6];
        public int[] I_E_pipeline_RD = new int[6];
        public int[] I_E_pipeline_immediate = new int[6];

       

        //EX/MEM pipeline register to hold information
        //in between execute and memory stages

        //holds numbers of registers not values
        public string[] E_M_pipeline_instruction = new string[6];
        public int[] E_M_pipeline_RS = new int[6];
        public int[] E_M_pipeline_RT = new int[6];
        public int[] E_M_pipeline_RD = new int[6];
        public int[] E_M_pipeline_immediate = new int[6];

        //holds actual values of registers
        public int[] EMpipe_RS_value = new int[6];
        public int[] EMpipe_RT_value = new int[6];
        public int[] EMpipe_RD_value = new int[6];

      
        //MEM/WB pipeline register to hold information between 
        //memory and write back stages
        public string[] M_W_pipeline_instruction = new string[6];
        public int[] M_W_pipeline_RS = new int[6];
        public int[] M_W_pipeline_RT = new int[6];
        public int[] M_W_pipeline_RD = new int[6];
        public int[] M_W_pipeline_immediate = new int[6];

        //holds actual values of registers
        public int[] MWpipe_RS_value = new int[6];
        public int[] MWpipe_RT_value = new int[6];
        public int[] MWpipe_RD_value = new int[6];

        //holds memory addresses from LW and SW
        public int[] LW_SW_memory_address = new int[6];

        struct registerSim{  //used value to use struct at value type for array of structs
		    public static int[] value = new int [32]; //values stored in register; will be hexadecimal; static to retain values in between calls
			public static string[] regName = new string[32]; // stores name of register; size 6 to accommodate the null terminating character after $zero
		};

       //uses array for memory block with 1024 bytes of storage
        public static int[] memory_value = new int[256];

        //used to hold old value in memory
        public static int[] old_memory_values = new int[6];
        
        //pipeline instruction variables
        //used to properly identify which stage each instruction is in.
        //used in switch statements
        public static int instruction_pipeline_1 = 0;
        public static int instruction_pipeline_2 = 0;
        public static int instruction_pipeline_3 = 0;
        public static int instruction_pipeline_4 = 0;
        public static int instruction_pipeline_5 = 0;
        public static int instruction_pipeline_6 = 0;

        //used to dictate when instruction is to be started during pipelining
        public static int instruction_pipeline_chk_2 = 0;
        public static int instruction_pipeline_chk_3 = 0;
        public static int instruction_pipeline_chk_4 = 0;
        public static int instruction_pipeline_chk_5 = 0;
        public static int instruction_pipeline_chk_6 = 0;
       
        //used to help with pipelining instructions
        public static int start_instruction_3 = 0;
        public static int start_instruction_4 = 0;
        public static int start_instruction_5 = 0;
        public static int start_instruction_6 = 0;

        public Form1()
        {
            InitializeComponent();
            
            //ensures that step_btn is disabled until start_btn is pressed
            step_btn.Enabled = false;

            //ensures that run_btn is disabled until start_btn is pressed
            run_btn.Enabled = false;

            //ensures that register zero is "hard wired" to zero
            registerSim.value[0] = 0;

            //calls random_register_values() function
            random_register_values();

            //calls random_memory_values() function
            random_memory_values();
           
            //assigns names to appropriate registers
            registerSim.regName[0] = "$zero";
            registerSim.regName[1]="$at";
            registerSim.regName[2]="$v0";
            registerSim.regName[3]="$v1";
            registerSim.regName[4]="$a0";
            registerSim.regName[5]="$a1";
            registerSim.regName[6]="$a2";
            registerSim.regName[7]="$a3";
            registerSim.regName[8]="$t0";
            registerSim.regName[9]="$t1";
            registerSim.regName[10]="$t2";
            registerSim.regName[11]="$t3";
            registerSim.regName[12]="$t4";
            registerSim.regName[13]="$t5";
            registerSim.regName[14]="$t6";
            registerSim.regName[15]="$t7";
            registerSim.regName[16]="$s0";
            registerSim.regName[17]="$s1";
            registerSim.regName[18]="$s2";
            registerSim.regName[19]="$s3";
            registerSim.regName[20]="$s4";
            registerSim.regName[21]="$s5";
            registerSim.regName[22]="$s6";
            registerSim.regName[23]="$s7";
            registerSim.regName[24]="$t8";
            registerSim.regName[25]="$t9";
            registerSim.regName[26]="$k0";
            registerSim.regName[27]="$k1";
            registerSim.regName[28]="$gp";
            registerSim.regName[29]="$sp";
            registerSim.regName[30]="$fp";
            registerSim.regName[31]="$ra";
        }


        private void button1_Click(object sender, EventArgs e)
        {
            int instruction_length = instr_input_textBox.Text.Length;
            int letter_check, length_check; 
            
            letter_check=letter_validation();
            length_check= length_validation(instruction_length);
            input_Instruction(length_check, letter_check);
            disable_AddButton();
        }

        //checks to see if instruction is long or short enough for proper execution
        private int length_validation(int instruction_length)
        {
            int length_check = 0;
            if (instruction_length != 8)
            {
                MessageBox.Show("Instruction is either more or less than 32 bits.\nTry again.");
                instr_input_textBox.Text = "";
                length_check++;
            }
            return length_check;
        }

        //checks to see if all letters are CAPITALIZED
        private int letter_validation()
        {
            int letter_check=0;
            int check_count=0;
            char[]temp_letters=instr_input_textBox.Text.ToCharArray();

            for(int i = 0; i < instr_input_textBox.Text.Length;i++)
            {
                if (temp_letters[i] != 'A' && temp_letters[i] != 'B' && temp_letters[i] != 'C' && temp_letters[i] != 'D'
                    && temp_letters[i] != 'E' && temp_letters[i] != 'F' && !Char.IsDigit(temp_letters[i]))
                {
                MessageBox.Show("Invalid value found in instruction. \nTry Again.");
                instr_input_textBox.Text = "";
                check_count++;
                letter_check++;
                }    
            }
            return letter_check;
        }

        //inputs instruction into listbox
        private void input_Instruction(int length_check, int letter_check)
        {
            if (length_check == 0 && letter_check==0)
            {
                hexInstructions.Add(instr_input_textBox.Text);
                instr_hold_box.Items.Add(hexInstructions[instruction_count]);
                instruction_count++;
                instr_input_textBox.Text = "";
                instr_counter.Items.Clear();
                instr_counter.Items.Add(instruction_count);
            }
        }

        //disables the Add Instruction button if conditions are met
        private void disable_AddButton()
        {
            //
            //limits the amount of instructions to be entered to 6
            //project requirement
            //
            if(instruction_count>=6)
            {
                button1.Enabled = false;
            }
        }

        //used to populate memory and GPR (names and values) lists
        private void start_btn_Click(object sender, EventArgs e)
        {
            //calls populate_GPR_Values() function
            populate_GPR_Values();
            
            //calls populate_GPR_List() function
            populate_GPR_List();

            //calls populate_memory_list() function
            populate_memory_list();
            start_btn.Enabled = false;
            step_btn.Enabled = true;
            run_btn.Enabled = true;
        }

        //used to step through clock cycles individually
        private void decode_btn_Click(object sender, EventArgs e)
        {
            if (hexInstructions.Count() != 0)
            {
                //makes sure that no other instructions can be added
                //once decoding process has begun
                button1.Enabled = false;

                //starts clock cycle out at 1
                clock_cycle++;

                //clears clock cycle box
                clock_cycle_box.Items.Clear();

                //adds current clock cycle to box
                clock_cycle_box.Items.Add(clock_cycle.ToString());

                Test_Status.Items.Add("**********************************");
                Test_Status.Items.Add("Clock Cycle: " + clock_cycle.ToString());
                Test_Status.Items.Add("**********************************");
                if (hexInstructions.Count() >= 1)
                {
                    instruction_cycle(instruction_pipeline_1, 0);
                    instruction_pipeline_1++;
                    disable_step_btn_check();
                }

                if (hexInstructions.Count() >= 2 && instruction_pipeline_chk_2 != 0)
                {
                    instruction_cycle(instruction_pipeline_2, 1);
                    instruction_pipeline_2++;
                    disable_step_btn_check();
                    j = 6;
                }

                if (hexInstructions.Count() >= 3 && instruction_pipeline_chk_3 != 0)
                {
                    instruction_cycle(instruction_pipeline_3, 2);
                    instruction_pipeline_3++;
                    disable_step_btn_check();
                    j = 7;
                }

                if (hexInstructions.Count() >= 4 && instruction_pipeline_chk_4 != 0)
                {
                    instruction_cycle(instruction_pipeline_4, 3);
                    instruction_pipeline_4++;
                    disable_step_btn_check();
                    j = 8;
                }

                if (hexInstructions.Count() >= 5 && instruction_pipeline_chk_5 != 0)
                {
                    instruction_cycle(instruction_pipeline_5, 4);
                    instruction_pipeline_5++;
                    disable_step_btn_check();
                    j = 9;
                }

                if (hexInstructions.Count() >= 6 && instruction_pipeline_chk_6 != 0)
                {
                    instruction_cycle(instruction_pipeline_6, 5);
                    instruction_pipeline_6++;
                    disable_step_btn_check();
                    j = 10;
                }

                i++;
                Test_Status.Items.Add("**********************************");
                check_if_next_instruction();
            }

            else
            {
                MessageBox.Show("No instructions have been added.");
            }
        }
        
        //used to run to last clock cycle at once
        private void run_btn_Click(object sender, EventArgs e)
        {
            if (hexInstructions.Count() != 0)
            {
                //makes sure that no other instructions can be added
                //once decoding process has begun
                button1.Enabled = false;

                do
                {
                    //starts clock cycle out at 1
                    clock_cycle++;

                    //clears clock cycle box
                    clock_cycle_box.Items.Clear();

                    //adds current clock cycle to box
                    clock_cycle_box.Items.Add(clock_cycle.ToString());

                    Test_Status.Items.Add("**********************************");
                    Test_Status.Items.Add("Clock Cycle: " + clock_cycle.ToString());
                    Test_Status.Items.Add("**********************************");
                    if (hexInstructions.Count() >= 1)
                    {
                        instruction_cycle(instruction_pipeline_1, 0);
                        instruction_pipeline_1++;
                        disable_step_btn_check();
                    }

                    if (hexInstructions.Count() >= 2 && instruction_pipeline_chk_2 != 0)
                    {
                        instruction_cycle(instruction_pipeline_2, 1);
                        instruction_pipeline_2++;
                        disable_step_btn_check();
                        j = 6;
                    }

                    if (hexInstructions.Count() >= 3 && instruction_pipeline_chk_3 != 0)
                    {
                        instruction_cycle(instruction_pipeline_3, 2);
                        instruction_pipeline_3++;
                        disable_step_btn_check();
                        j = 7;
                    }

                    if (hexInstructions.Count() >= 4 && instruction_pipeline_chk_4 != 0)
                    {
                        instruction_cycle(instruction_pipeline_4, 3);
                        instruction_pipeline_4++;
                        disable_step_btn_check();
                        j = 8;
                    }

                    if (hexInstructions.Count() >= 5 && instruction_pipeline_chk_5 != 0)
                    {
                        instruction_cycle(instruction_pipeline_5, 4);
                        instruction_pipeline_5++;
                        disable_step_btn_check();
                        j = 9;
                    }

                    if (hexInstructions.Count() >= 6 && instruction_pipeline_chk_6 != 0)
                    {
                        instruction_cycle(instruction_pipeline_6, 5);
                        instruction_pipeline_6++;
                        disable_step_btn_check();
                        j = 10;
                    }
                    i++;

                    Test_Status.Items.Add("**********************************");
                    check_if_next_instruction();
                } while (i != j);
            }
            else
            {
                MessageBox.Show("No instructions have been added.");
            }
        }
        
        //populates list box with values in registerSim.value[] array
        private void populate_GPR_Values()
        {
            for (int i = 0; i < 32; i++)
            {
                GPR_Values.Items.Add("R" + i + "--" + registerSim.value[i]);
            }
        }
        
        //populates list box with values in registerSim.regName List
        private void populate_GPR_List()
         {
             for (int i = 0; i < 32; i++)
             {
                 GPR_List.Items.Add("R" + i + "--" + registerSim.regName[i]);
             }
         }  
           
        //populates list box with values in memory_value[] array
        private void populate_memory_list()
        {
            for (int i = 0; i < 256; i++)
            {
                memory_block_List.Items.Add(i.ToString("D8") + ": " + memory_value[i]);
            }
        }    
        
        //insert random values between 1 and 10 into registerSim value variable
        private void random_register_values()
        {
            //needed for random numbers
            Random r = new Random();

            for (int i = 1; i < 32; ++i)
            {
                registerSim.value[i] = r.Next(1, 11);
            }
        }

        //insert random values between 0 and 5 into memory memory_value variable
        private void random_memory_values()
        {
            //needed for random numbers
            Random r = new Random();

            
            for (int i = 0; i < 256; ++i)
            {
                memory_value[i] = r.Next(1, 6);
            }
        }
        
        //checks to see if there is another instruction that needs to be added to pipelining
        private void check_if_next_instruction()
        {
            
            //checks to see if there are more than 1 instructions in hexInstructions
            //if so it increases instruction_pipeline_chk_2 to allow instruction 2 to begin
            if (hexInstructions.Count() > 1)
            {
                instruction_pipeline_chk_2++;
            }

            //checks to see if there are more than 2 instructions in hexInstructions
            //& if start_instruction_3 has been incremented.
            //if so it increases instruction_pipeline_chk_3 to allow instruction 3 to begin
            //if conditions have not been met then there are less than 3 instructions
            if (hexInstructions.Count() > 2 && start_instruction_3!=0)
            {
                instruction_pipeline_chk_3++;
            }

            //checks to see if there are more than 3 instructions in hexInstructions
            //& if start_instruction_4 has been incremented.
            //if so it increases instruction_pipeline_chk_4 to allow instruction 4 to begin
            //if conditions have not been met then there are less than 4 instructions
            if (hexInstructions.Count() > 3 && start_instruction_4 != 0)
            {
                instruction_pipeline_chk_4++;
            }

            //checks to see if there are more than 4 instructions in hexInstructions
            //& if start_instruction_5 has been incremented.
            //if so it increases instruction_pipeline_chk_5 to allow instruction 5 to begin
            //if conditions have not been met then there are less than 5 instructions
            if (hexInstructions.Count() > 4 && start_instruction_5 != 0)
            {
                instruction_pipeline_chk_5++;
            }

            //checks to see if there are more than 5 instructions in hexInstructions
            //& if start_instruction_6 has been incremented.
            //if so it increases instruction_pipeline_chk_6 to allow instruction 6 to begin
            //if conditions have not been met then there are less than 6 instructions
            if (hexInstructions.Count() > 5  && start_instruction_6 != 0)
            {
                instruction_pipeline_chk_6++;
            }


            //the following if statements help with the pipelining conditions

            //prevents instructions 2 and 3 from being started during the same clock cycle
            if (instruction_pipeline_chk_2 != 0)
            {
                start_instruction_3++;
            }

            //prevents instructions 3 and 4 from being started during the same clock cycle
            if (instruction_pipeline_chk_3 != 0)
            {
                start_instruction_4++;
            }

            //prevents instructions 4 and 5 from being started during the same clock cycle
            if (instruction_pipeline_chk_4 != 0)
            {
                start_instruction_5++;
            }

            //prevents instructions 5 and 6 from being started during the same clock cycle
            if (instruction_pipeline_chk_5 != 0)
            {
                start_instruction_6++;
            }
        }

        //checks conditions to know when to disable Step button
        private void disable_step_btn_check()
        {
            if (hexInstructions.Count() == 1 && instruction_pipeline_1 == 5)
            {
                step_btn.Enabled = false;
            }

            if (hexInstructions.Count() == 2 && instruction_pipeline_2 == 5)
            {
                step_btn.Enabled = false;
            }

            if (hexInstructions.Count() == 3 && instruction_pipeline_3 == 5)
            {
                step_btn.Enabled = false;
            }

            if (hexInstructions.Count() == 4 && instruction_pipeline_4 == 5)
            {
                step_btn.Enabled = false;
            }

            if (hexInstructions.Count() == 5 && instruction_pipeline_5 == 5)
            {
                step_btn.Enabled = false;
            }

            if (hexInstructions.Count() == 6 && instruction_pipeline_6 == 5)
            {
                step_btn.Enabled = false;
            }

        }

        //checks to see which part of instruction cycle needs to be executed
        private void instruction_cycle(int instruction_pipeline, int instruction_index)
        {
            switch (instruction_pipeline)
            {
                case 0: instruction_fetch(instruction_index);
                    break;
                case 1: instruction_decode(instruction_index);
                    break;
                case 2: execute(instruction_index);
                    break;
                case 3: memory_stage(instruction_index);
                    break;
                case 4: write_back(instruction_index);
                    break;
            }
        }

        /* 3 conversion functions.
         1 converts from hexadecimal to binary. returns string with binary value
         1 converts from binary to decimal value. returns int with decimal value
         1 converts from decimal value to binary. returns char[] with binary value
         */
        private string hexToBinary(int instruction_index)
        {
            char[] temp_hex = if_id_pipeline[instruction_index].ToCharArray();
            string temp_binary = "";
            for (int i = 0; i < 8; i++)
            {

                switch (temp_hex[i])
                {
                    case '0': temp_binary += "0";
                        temp_binary += "0";
                        temp_binary += "0";
                        temp_binary += "0";
                        break;
                    case '1': temp_binary += "0";
                        temp_binary += "0";
                        temp_binary += "0";
                        temp_binary = temp_binary += "1";
                        break;
                    case '2': temp_binary += "0";
                        temp_binary += "0";
                        temp_binary = temp_binary += "1";
                        temp_binary += "0";
                        break;
                    case '3': temp_binary += "0";
                        temp_binary += "0";
                        temp_binary = temp_binary += "1";
                        temp_binary = temp_binary += "1";
                        break;
                    case '4': temp_binary += "0";
                        temp_binary = temp_binary += "1";
                        temp_binary += "0";
                        temp_binary += "0";
                        break;
                    case '5': temp_binary += "0";
                        temp_binary = temp_binary += "1";
                        temp_binary += "0";
                        temp_binary = temp_binary += "1";
                        break;
                    case '6': temp_binary += "0";
                        temp_binary = temp_binary += "1";
                        temp_binary = temp_binary += "1";
                        temp_binary += "0";
                        break;
                    case '7': temp_binary += "0";
                        temp_binary = temp_binary += "1";
                        temp_binary = temp_binary += "1";
                        temp_binary = temp_binary += "1";
                        break;
                    case '8': temp_binary = temp_binary += "1";
                        temp_binary += "0";
                        temp_binary += "0";
                        temp_binary += "0";
                        break;
                    case '9': temp_binary = temp_binary += "1";
                        temp_binary += "0";
                        temp_binary += "0";
                        temp_binary = temp_binary += "1";
                        break;
                    case 'A': temp_binary = temp_binary += "1";
                        temp_binary += "0";
                        temp_binary = temp_binary += "1";
                        temp_binary += "0";
                        break;
                    case 'B': temp_binary = temp_binary += "1";
                        temp_binary += "0";
                        temp_binary = temp_binary += "1";
                        temp_binary = temp_binary += "1";
                        break;
                    case 'C': temp_binary = temp_binary += "1";
                        temp_binary = temp_binary += "1";
                        temp_binary += "0";
                        temp_binary += "0";
                        break;
                    case 'D': temp_binary = temp_binary += "1";
                        temp_binary = temp_binary += "1";
                        temp_binary += "0";
                        temp_binary = temp_binary += "1";
                        break;
                    case 'E': temp_binary = temp_binary += "1";
                        temp_binary = temp_binary += "1";
                        temp_binary = temp_binary += "1";
                        temp_binary += "0";
                        break;
                    case 'F': temp_binary = temp_binary += "1";
                        temp_binary = temp_binary += "1";
                        temp_binary = temp_binary += "1";
                        temp_binary = temp_binary += "1";
                        break;
                }
            }
            return temp_binary;
        }

        private int binaryToDecimal(char[] temp_if_id_pipeline, int startIndex, int endIndex)
        {
            //used to hold value of final converted decimal value
            int decimal_number = 0;

            //for power of exponent for Math.Pow() function
            int exponent_power = 0;

            //used to temporarily hold binary value being converted to decimal
            int temp_binary = 0;

            //converts specific part of full instruction to smaller portion to be converted
            //part to be converted is in between startIndex and endIndex
            for (int i = startIndex; i < endIndex; i++)
            {
                temp_binary *= 10;

                if (temp_if_id_pipeline[i] == '0')
                {
                    temp_binary += 0;
                }
                else if (temp_if_id_pipeline[i] == '1')
                {
                    temp_binary += 1;
                }

            }

            //converts temp_binary value to decimal number
            while (true)
            {
                if (temp_binary == 0)
                {
                    break;
                }
                else
                {
                    int decimal_num_place = temp_binary % 10;
                    decimal_number += decimal_num_place * (int)Math.Pow(2, exponent_power);
                    temp_binary /= 10;
                    exponent_power++;
                }
            }
            return decimal_number;
        }

        private char[] decimalToBinary(int number)
        {
            string temp_binary = "";

            int count = 0;
            while (count < 5)
            {
                temp_binary+=(number % 2);
                number /= 2;
                count++;
            }

            char[] temp_binary_char=temp_binary.ToCharArray();
            Array.Reverse(temp_binary_char);

            return temp_binary_char;
        }


        /*Pipeline stage functions. 5 in total*/
        private void instruction_fetch(int instruction_index)
        {
            string hexadecimal_prog_counter;

            hexadecimal_prog_counter = program_counter.ToString("X8");
            //shows current address of instruction in memory
            prog_counter_box.Items.Clear();
            prog_counter_box.Items.Add(hexadecimal_prog_counter);

            //write instruction to IF/ID pipeline register
            if_id_pipeline[instruction_index] = hexInstructions[instruction_index];

            //increments program counter by 4
            program_counter += 4;

            Test_Status.Items.Add("Instruction Fetch (Instruction " + (instruction_index + 1) + ")");
            Test_Status.Items.Add("--------------------");
            Test_Status.Items.Add("Instruction " + (instruction_index + 1) + ": " + if_id_pipeline[instruction_index]);
            Test_Status.Items.Add("");
        }

        private void instruction_decode(int instruction_index)
        {

            //converts if_id_pipeline to charArray
            char[] temp_hex = if_id_pipeline[instruction_index].ToCharArray();

            //converts if_id_pipeline from Hexadecimal to Binary
            string temp_binary = hexToBinary(instruction_index);

            //uses temp_binary to get particular instruction
            string instruction = instruction_type(temp_binary);
            
            //calls instruction_info to set value info into ID/EX pipeline register
            instruction_info(instruction, temp_binary, instruction_index);


            Test_Status.Items.Add("Instruction Decode (Instruction " + (instruction_index + 1) + ")");
            Test_Status.Items.Add("--------------------");
            Test_Status.Items.Add("Instruction: "+instruction);
            Test_Status.Items.Add("");

        }

        private void execute(int instruction_index)
        {
            Test_Status.Items.Add("Execute (Instruction "+(instruction_index+1)+")");
            Test_Status.Items.Add("--------------------");
            switch (I_E_pipeline_instruction[instruction_index])
            {
                case "ADD": add(instruction_index);
                    Test_Status.Items.Add("RS Source Register R" + E_M_pipeline_RS[instruction_index] + " value: " + registerSim.value[E_M_pipeline_RS[instruction_index]]);
                    Test_Status.Items.Add("RT Source Register R" + E_M_pipeline_RT[instruction_index] + " value: " + registerSim.value[E_M_pipeline_RT[instruction_index]]);
                    Test_Status.Items.Add("New value for R"+E_M_pipeline_RD[instruction_index]+": " + EMpipe_RD_value[instruction_index]);
                    Test_Status.Items.Add("");
                    break;

                case "ADDI": addi(instruction_index);
                    Test_Status.Items.Add("RS Source Register R" + E_M_pipeline_RS[instruction_index] + " value: " + registerSim.value[E_M_pipeline_RS[instruction_index]]);
                    Test_Status.Items.Add("New value for R" + E_M_pipeline_RT[instruction_index] + ": " + EMpipe_RT_value[instruction_index]);
                    Test_Status.Items.Add("");
                    break;

                case "SUB": sub(instruction_index);
                    Test_Status.Items.Add("RS Source Register R" + E_M_pipeline_RS[instruction_index] + " value: " + registerSim.value[E_M_pipeline_RS[instruction_index]]);
                    Test_Status.Items.Add("RT Source Register R" + E_M_pipeline_RT[instruction_index] + " value: " + registerSim.value[E_M_pipeline_RT[instruction_index]]);
                    Test_Status.Items.Add("New value for R"+E_M_pipeline_RD[instruction_index]+": " + EMpipe_RD_value[instruction_index]);
                    Test_Status.Items.Add("");
                    break;

                case "AND": and(instruction_index);
                    Test_Status.Items.Add("RS Source Register R" + E_M_pipeline_RS[instruction_index] + " value: " + registerSim.value[E_M_pipeline_RS[instruction_index]]);
                    Test_Status.Items.Add("RT Source Register R" + E_M_pipeline_RT[instruction_index] + " value: " + registerSim.value[E_M_pipeline_RT[instruction_index]]);
                    Test_Status.Items.Add("New value for R"+E_M_pipeline_RD[instruction_index]+": " + EMpipe_RD_value[instruction_index]);
                    Test_Status.Items.Add("");
                    break;
                case "OR": or(instruction_index);
                    Test_Status.Items.Add("RS Source Register R" + E_M_pipeline_RS[instruction_index] + " value: " + registerSim.value[E_M_pipeline_RS[instruction_index]]);
                    Test_Status.Items.Add("RT Source Register R" + E_M_pipeline_RT[instruction_index] + " value: " + registerSim.value[E_M_pipeline_RT[instruction_index]]);
                    Test_Status.Items.Add("New value for R" + E_M_pipeline_RD[instruction_index] + ": " + EMpipe_RD_value[instruction_index]);
                    Test_Status.Items.Add("");
                    break;
                case "SLT": slt(instruction_index);
                    Test_Status.Items.Add("RS Source Register R" + E_M_pipeline_RS[instruction_index] + " value: " + registerSim.value[E_M_pipeline_RS[instruction_index]]);
                    Test_Status.Items.Add("RT Source Register R" + E_M_pipeline_RT[instruction_index] + " value: " + registerSim.value[E_M_pipeline_RT[instruction_index]]);
                    Test_Status.Items.Add("New value for R" + E_M_pipeline_RD[instruction_index] + ": " + EMpipe_RD_value[instruction_index]);
                    Test_Status.Items.Add("");
                    break;
                case "NOP": nop(instruction_index);
                    Test_Status.Items.Add(E_M_pipeline_instruction[instruction_index]);
                    Test_Status.Items.Add("");
                    break;
                case "LW": lw_ex(instruction_index);
                    Test_Status.Items.Add("Source Register (needed for memory address): R" + E_M_pipeline_RS[instruction_index]);
                    Test_Status.Items.Add("Offest value (needed for memory address): " + E_M_pipeline_immediate[instruction_index]);
                    Test_Status.Items.Add("Value will be loaded from memory address: " + (E_M_pipeline_RS[instruction_index] + E_M_pipeline_immediate[instruction_index]).ToString("D8"));
                    Test_Status.Items.Add("Destination Regster: R" + E_M_pipeline_RT[instruction_index]);
                    Test_Status.Items.Add("");
                    break;
                case "SW": sw_ex(instruction_index);
                    Test_Status.Items.Add("Source Register (needed for memory address): R" + E_M_pipeline_RS[instruction_index]);
                    Test_Status.Items.Add("Offest value (needed for memory address): " + E_M_pipeline_immediate[instruction_index]);
                    Test_Status.Items.Add("Destination Register (value will be taken from here): R" + E_M_pipeline_RT[instruction_index]);
                    Test_Status.Items.Add("Value will be stored in destination memory address: " + (E_M_pipeline_RS[instruction_index] + E_M_pipeline_immediate[instruction_index]).ToString("D8"));
                    Test_Status.Items.Add("");
                    break;


            }

        }

        private void memory_stage(int instruction_index)
        {
            M_W_pipeline_RT[instruction_index] = E_M_pipeline_RT[instruction_index];
            M_W_pipeline_RD[instruction_index] = E_M_pipeline_RD[instruction_index];
            MWpipe_RD_value[instruction_index] = EMpipe_RD_value[instruction_index];
            MWpipe_RT_value[instruction_index] = EMpipe_RT_value[instruction_index];
            M_W_pipeline_instruction[instruction_index] = E_M_pipeline_instruction[instruction_index];

            Test_Status.Items.Add("Memory (Instruction " + (instruction_index + 1) + ")");
            Test_Status.Items.Add("--------------------");
            switch (E_M_pipeline_instruction[instruction_index])
            {
                case "LW": lw_mem(instruction_index);
                    Test_Status.Items.Add("New value for R"+M_W_pipeline_RT[instruction_index]+": " + MWpipe_RT_value[instruction_index]);
                    Test_Status.Items.Add("");
                    break;
                case "SW": sw_mem(instruction_index);
                    Test_Status.Items.Add("New value for memory address "+LW_SW_memory_address[instruction_index].ToString("D8")+": " + registerSim.value[M_W_pipeline_RT[instruction_index]]);
                    Test_Status.Items.Add("");
                    break;
                default: Test_Status.Items.Add("Nothing is done during this stage for instruction: "+M_W_pipeline_instruction[instruction_index]);
                    Test_Status.Items.Add("");
                    break;
            }
            
        }

        private void write_back(int instruction_index)
        {
            Test_Status.Items.Add("Write Back (Instruction " + (instruction_index + 1) + ")");
            Test_Status.Items.Add("--------------------");
            switch (M_W_pipeline_instruction[instruction_index])
            {
                case "ADD": Test_Status.Items.Add("Old value of R"+M_W_pipeline_RD[instruction_index]+": "+registerSim.value[M_W_pipeline_RD[instruction_index]]);
                    registerSim.value[M_W_pipeline_RD[instruction_index]] = MWpipe_RD_value[instruction_index];
                    Test_Status.Items.Add("New value of R"+M_W_pipeline_RD[instruction_index]+": "+registerSim.value[M_W_pipeline_RD[instruction_index]]+" is now stored.");
                    Test_Status.Items.Add("");
                    GPR_Values.Items.Clear();
                    populate_GPR_Values();
                    break;

                case "SUB": Test_Status.Items.Add("Old value of R"+M_W_pipeline_RD[instruction_index]+": "+registerSim.value[M_W_pipeline_RD[instruction_index]]);
                    registerSim.value[M_W_pipeline_RD[instruction_index]] = MWpipe_RD_value[instruction_index];
                    Test_Status.Items.Add("New value of R"+M_W_pipeline_RD[instruction_index]+": "+registerSim.value[M_W_pipeline_RD[instruction_index]]+" is now stored.");
                    Test_Status.Items.Add("");
                    GPR_Values.Items.Clear();
                    populate_GPR_Values();
                    break;

                case "AND": Test_Status.Items.Add("Old value of R" + M_W_pipeline_RD[instruction_index] + ": " + registerSim.value[M_W_pipeline_RD[instruction_index]]);
                    registerSim.value[M_W_pipeline_RD[instruction_index]] = MWpipe_RD_value[instruction_index];
                    Test_Status.Items.Add("New value of R" + M_W_pipeline_RD[instruction_index] + ": " + registerSim.value[M_W_pipeline_RD[instruction_index]] + " is now stored.");
                    Test_Status.Items.Add("");
                    GPR_Values.Items.Clear();
                    populate_GPR_Values();
                    break;

                case "OR": Test_Status.Items.Add("Old value of R" + M_W_pipeline_RD[instruction_index] + ": " + registerSim.value[M_W_pipeline_RD[instruction_index]]);
                    registerSim.value[M_W_pipeline_RD[instruction_index]] = MWpipe_RD_value[instruction_index];
                    Test_Status.Items.Add("New value of R" + M_W_pipeline_RD[instruction_index] + ": " + registerSim.value[M_W_pipeline_RD[instruction_index]] + " is now stored.");
                    Test_Status.Items.Add("");
                    GPR_Values.Items.Clear();
                    populate_GPR_Values();
                    break;

                case "SLT": Test_Status.Items.Add("Old value of R" + M_W_pipeline_RD[instruction_index] + ": " + registerSim.value[M_W_pipeline_RD[instruction_index]]);
                    registerSim.value[M_W_pipeline_RD[instruction_index]] = MWpipe_RD_value[instruction_index];
                    Test_Status.Items.Add("New value of R" + M_W_pipeline_RD[instruction_index] + ": " + registerSim.value[M_W_pipeline_RD[instruction_index]] + " is now stored.");
                    Test_Status.Items.Add("");
                    GPR_Values.Items.Clear();
                    populate_GPR_Values();
                    break;

                case "NOP": Test_Status.Items.Add("NOP");
                    Test_Status.Items.Add("");
                    GPR_Values.Items.Clear();
                    populate_GPR_Values();
                    break;

                case "LW": Test_Status.Items.Add("Old value of R" + M_W_pipeline_RT[instruction_index] + ": " + registerSim.value[M_W_pipeline_RT[instruction_index]]);
                    registerSim.value[M_W_pipeline_RT[instruction_index]] = memory_value[LW_SW_memory_address[instruction_index]];
                    Test_Status.Items.Add("New value of R" + M_W_pipeline_RT[instruction_index] + ": " + registerSim.value[M_W_pipeline_RT[instruction_index]] + " is now stored.");
                    Test_Status.Items.Add("");
                    GPR_Values.Items.Clear();
                    populate_GPR_Values();
                    break;

                case "SW": Test_Status.Items.Add("Old value of memory address" + LW_SW_memory_address[instruction_index].ToString("D8") + ": " + old_memory_values[instruction_index]);
                    Test_Status.Items.Add("New value of memory address" + LW_SW_memory_address[instruction_index].ToString("D8") + ": " + memory_value[LW_SW_memory_address[instruction_index]]);
                    Test_Status.Items.Add("");
                    memory_block_List.Items.Clear();
                    populate_memory_list();
                    break;

                case "ADDI": Test_Status.Items.Add("Old value of R" + M_W_pipeline_RT[instruction_index] + ": " + registerSim.value[M_W_pipeline_RT[instruction_index]]);
                    registerSim.value[M_W_pipeline_RT[instruction_index]] = MWpipe_RT_value[instruction_index];
                    Test_Status.Items.Add("New value of R" + M_W_pipeline_RT[instruction_index] + ": " + registerSim.value[M_W_pipeline_RT[instruction_index]] + " is now stored.");
                    Test_Status.Items.Add("");
                    GPR_Values.Items.Clear();
                    populate_GPR_Values();
                    break;
            }
        }


        /*2 sub functions for instruction_decode()*/
        private string instruction_type(string temp_binary)
        {
            //string will hold op code of instruction
            string opCode = "";

            //int will hold function code of instruction if needed
            int function_code;

            //will hold instruction. will be returned at end of instruction_type()
            string instruction="";

            //converts temp_binary value into charArray for functions
            char[] temp_if_id_pipeline = temp_binary.ToCharArray();

            //determines opCode of instruction
            for (int i = 0; i < 6; i++)
            {
                opCode+=temp_if_id_pipeline[i].ToString();
            }

            if (opCode == "000000")
            {
                //calls binaryToDecimal function and stores value into function_code
                function_code = binaryToDecimal(temp_if_id_pipeline, 26, 32);

                switch (function_code)
                {
                    case 0: instruction = "NOP";
                        break;
                    case 32: instruction = "ADD";
                        break;
                    case 34: instruction = "SUB";
                        break;
                    case 36: instruction = "AND";
                        break;
                    case 37: instruction = "OR";
                        break;
                    case 42: instruction = "SLT";
                        break;
                    default: MessageBox.Show("Not a valid function code");
                        break;
                }
            }

            else if (opCode == "001000")
            {
                instruction = "ADDI";
            }

            else if(opCode=="100011")
            {
                instruction = "LW";
            }

            else if (opCode == "101011")
            {
                instruction = "SW";
            }
            
            return instruction;

        }

        private void instruction_info(string instruction, string temp_binary, int instruction_index)
        {
            char[] temp_hex = temp_binary.ToCharArray();
            switch (instruction)
            {
                case "ADD": add_set(temp_hex, instruction_index);
                    break;
                case "SUB": sub_set(temp_hex, instruction_index);
                    break;
                case "AND": and_set(temp_hex, instruction_index);
                    break;
                case "OR": or_set(temp_hex, instruction_index);
                    break;
                case "SLT": slt_set(temp_hex, instruction_index);
                    break;
                case "NOP": nop_set(temp_hex, instruction_index);
                    break;
                case "ADDI": addi_set(temp_hex, instruction_index);
                    break;
                case "SW": sw_set(temp_hex, instruction_index);
                    break;
                case "LW": lw_set(temp_hex, instruction_index);
                    break;

            }
        }


        /*all function that are _set simply store info into id/ex pipeline register
         the DO NOT perform the actual instruction*/
        private void addi_set(char[] temp_hex, int instruction_index) 
        {
            I_E_pipeline_RS[instruction_index] = binaryToDecimal(temp_hex, 6, 11);
            I_E_pipeline_RT[instruction_index] = binaryToDecimal(temp_hex, 11, 16);
            I_E_pipeline_immediate[instruction_index] = binaryToDecimal(temp_hex, 16, 32);
            I_E_pipeline_instruction[instruction_index] = "ADDI";
        }

        private void add_set(char[] temp_hex, int instruction_index)
        {
             
            I_E_pipeline_RS[instruction_index] = binaryToDecimal(temp_hex, 6, 11);
            I_E_pipeline_RT[instruction_index] = binaryToDecimal(temp_hex, 11, 16);
            I_E_pipeline_RD[instruction_index] = binaryToDecimal(temp_hex, 16, 21);
            I_E_pipeline_instruction[instruction_index] = "ADD";
        }

        private void sub_set(char[] temp_hex, int instruction_index)
        {
             
            I_E_pipeline_RS[instruction_index] = binaryToDecimal(temp_hex, 6, 11);
            I_E_pipeline_RT[instruction_index] = binaryToDecimal(temp_hex, 11, 16);
            I_E_pipeline_RD[instruction_index] = binaryToDecimal(temp_hex, 16, 21);
            I_E_pipeline_instruction[instruction_index] = "SUB";
        }

        private void and_set(char[] temp_hex, int instruction_index)
        {
             
            I_E_pipeline_RS[instruction_index] = binaryToDecimal(temp_hex, 6, 11);
            I_E_pipeline_RT[instruction_index] = binaryToDecimal(temp_hex, 11, 16);
            I_E_pipeline_RD[instruction_index] = binaryToDecimal(temp_hex, 16, 21);
            I_E_pipeline_instruction[instruction_index] = "AND";
        }

        private void or_set(char[] temp_hex, int instruction_index)
        {
             
            I_E_pipeline_RS[instruction_index] = binaryToDecimal(temp_hex, 6, 11);
            I_E_pipeline_RT[instruction_index] = binaryToDecimal(temp_hex, 11, 16);
            I_E_pipeline_RD[instruction_index] = binaryToDecimal(temp_hex, 16, 21);
            I_E_pipeline_instruction[instruction_index] = "OR";
        }

        private void slt_set(char[] temp_hex, int instruction_index)
        {
             
            I_E_pipeline_RS[instruction_index] = binaryToDecimal(temp_hex, 6, 11);
            I_E_pipeline_RT[instruction_index] = binaryToDecimal(temp_hex, 11, 16);
            I_E_pipeline_RD[instruction_index] = binaryToDecimal(temp_hex, 16, 21);
            I_E_pipeline_instruction[instruction_index] = "SLT";
        }

        private void nop_set(char[] temp_hex, int instruction_index)
        {
             
            I_E_pipeline_instruction[instruction_index] = "NOP";
        }

        private void sw_set(char[] temp_hex, int instruction_index)
        {
             
            I_E_pipeline_RS[instruction_index] = binaryToDecimal(temp_hex, 6, 11);
            I_E_pipeline_RT[instruction_index] = binaryToDecimal(temp_hex, 11, 16);
            I_E_pipeline_immediate[instruction_index] = binaryToDecimal(temp_hex, 16, 32);
            I_E_pipeline_instruction[instruction_index] = "SW";
        }

        private void lw_set(char[] temp_hex, int instruction_index)
        {
             
            I_E_pipeline_RS[instruction_index] = binaryToDecimal(temp_hex, 6, 11);
            I_E_pipeline_RT[instruction_index] = binaryToDecimal(temp_hex, 11, 16);
            I_E_pipeline_immediate[instruction_index] = binaryToDecimal(temp_hex, 16, 32);
           I_E_pipeline_instruction[instruction_index] = "LW";
        }

        /*functions below actually perform instruction operations */

        private void addi(int instruction_index)
        {
            //moves info from ID/EX pipeline register to EX/MEM pipeline register
            E_M_pipeline_RS[instruction_index] = I_E_pipeline_RS[instruction_index];
            E_M_pipeline_RT[instruction_index] = I_E_pipeline_RT[instruction_index];
            E_M_pipeline_immediate[instruction_index] = I_E_pipeline_immediate[instruction_index];
            E_M_pipeline_instruction[instruction_index] = I_E_pipeline_instruction[instruction_index];

            //performs ALU operation on registers. stores value into EX/MEM pipeline register
            EMpipe_RT_value[instruction_index] = registerSim.value[E_M_pipeline_RS[instruction_index]] + E_M_pipeline_immediate[instruction_index];

           
        }

        private void add(int instruction_index)
        {
            //moves info from ID/EX pipeline register to EX/MEM pipeline register
            E_M_pipeline_RD[instruction_index] = I_E_pipeline_RD[instruction_index];
            E_M_pipeline_RS[instruction_index] = I_E_pipeline_RS[instruction_index];
            E_M_pipeline_RT[instruction_index] = I_E_pipeline_RT[instruction_index];
            E_M_pipeline_instruction[instruction_index] = I_E_pipeline_instruction[instruction_index];

            //performs ALU operation on registers. stores value into EX/MEM pipeline register
            EMpipe_RD_value[instruction_index] = registerSim.value[E_M_pipeline_RS[instruction_index]] + registerSim.value[E_M_pipeline_RT[instruction_index]];
        }

        private void sub(int instruction_index)
        {
            //moves info from ID/EX pipeline register to EX/MEM pipeline register
            E_M_pipeline_RD[instruction_index] = I_E_pipeline_RD[instruction_index];
            E_M_pipeline_RS[instruction_index] = I_E_pipeline_RS[instruction_index];
            E_M_pipeline_RT[instruction_index] = I_E_pipeline_RT[instruction_index];
            E_M_pipeline_instruction[instruction_index] = I_E_pipeline_instruction[instruction_index];

            //performs ALU operation on registers. stores value into EX/MEM pipeline register
            EMpipe_RD_value[instruction_index] = registerSim.value[E_M_pipeline_RS[instruction_index]] - registerSim.value[E_M_pipeline_RT[instruction_index]];
        }

        private void and(int instruction_index)
        {
            //moves info from ID/EX pipeline register to EX/MEM pipeline register
            E_M_pipeline_RD[instruction_index] = I_E_pipeline_RD[instruction_index];
            E_M_pipeline_RS[instruction_index] = I_E_pipeline_RS[instruction_index];
            E_M_pipeline_RT[instruction_index] = I_E_pipeline_RT[instruction_index];
            E_M_pipeline_instruction[instruction_index] = I_E_pipeline_instruction[instruction_index];

            //used to perform AND operation
            char[] temp_RS_binary_value = decimalToBinary(registerSim.value[E_M_pipeline_RS[instruction_index]]);
            char[] temp_RT_binary_value = decimalToBinary(registerSim.value[E_M_pipeline_RT[instruction_index]]);
            char[] temp_RD_binary_value = new char[5];

            //performs ALU operation on registers. stores value into EX/MEM pipeline register
            for (int i = 0; i < 5; i++)
            {
                if (temp_RS_binary_value[i] == '1' && temp_RT_binary_value[i] == '1')
                {
                    temp_RD_binary_value[i] = '1';
                }
                else
                {
                    temp_RD_binary_value[i] = '0';
                }
            }

            //stores value into EX/MEM pipeline register
            EMpipe_RD_value[instruction_index] = binaryToDecimal(temp_RD_binary_value, 0, 5);
            
        }

        private void or(int instruction_index)
        {
            //moves info from ID/EX pipeline register to EX/MEM pipeline register
            E_M_pipeline_RD[instruction_index] = I_E_pipeline_RD[instruction_index];
            E_M_pipeline_RS[instruction_index] = I_E_pipeline_RS[instruction_index];
            E_M_pipeline_RT[instruction_index] = I_E_pipeline_RT[instruction_index];
            E_M_pipeline_instruction[instruction_index] = I_E_pipeline_instruction[instruction_index];

            //used to perform OR operation
            char[] temp_RS_binary_value = decimalToBinary(registerSim.value[E_M_pipeline_RS[instruction_index]]);
            char[] temp_RT_binary_value = decimalToBinary(registerSim.value[E_M_pipeline_RT[instruction_index]]);
            char[] temp_RD_binary_value = new char[5];

            //performs ALU operation on registers. stores value into EX/MEM pipeline register
            for (int i = 0; i < 5; i++)
            {
                if (temp_RS_binary_value[i] == '1' || temp_RT_binary_value[i] == '1')
                {
                    temp_RD_binary_value[i] = '1';
                }
                else
                {
                    temp_RD_binary_value[i] = '0';
                }
            }

            //stores value into EX/MEM pipeline register
            EMpipe_RD_value[instruction_index] = binaryToDecimal(temp_RD_binary_value, 0, 5);
        }

        private void slt(int instruction_index)
        {
            //moves info from ID/EX pipeline register to EX/MEM pipeline register
            E_M_pipeline_RD[instruction_index] = I_E_pipeline_RD[instruction_index];
            E_M_pipeline_RS[instruction_index] = I_E_pipeline_RS[instruction_index];
            E_M_pipeline_RT[instruction_index] = I_E_pipeline_RT[instruction_index];
            E_M_pipeline_instruction[instruction_index] = I_E_pipeline_instruction[instruction_index];

            //performs ALU operation on registers. stores value into EX/MEM pipeline register
            if (registerSim.value[E_M_pipeline_RS[instruction_index]] < registerSim.value[E_M_pipeline_RT[instruction_index]])
            {
                EMpipe_RD_value[instruction_index] = 1;
            }
            else
                EMpipe_RD_value[instruction_index] = 0;
        }

        private void nop(int instruction_index)
        {
            E_M_pipeline_instruction[instruction_index] = I_E_pipeline_instruction[instruction_index];
        }

        private void lw_ex(int instruction_index)
        {
            //moves info from ID/EX pipeline register to EX/MEM pipeline register
            E_M_pipeline_RS[instruction_index] = I_E_pipeline_RS[instruction_index];
            E_M_pipeline_RT[instruction_index] = I_E_pipeline_RT[instruction_index];
            E_M_pipeline_immediate[instruction_index] = I_E_pipeline_immediate[instruction_index];
            E_M_pipeline_instruction[instruction_index] = I_E_pipeline_instruction[instruction_index];

            //actual operation to be performed in lw_mem()

        }

        private void sw_ex(int instruction_index)
        {
            //moves info from ID/EX pipeline register to EX/MEM pipeline register
            E_M_pipeline_RS[instruction_index] = I_E_pipeline_RS[instruction_index];
            E_M_pipeline_RT[instruction_index] = I_E_pipeline_RT[instruction_index];
            E_M_pipeline_immediate[instruction_index] = I_E_pipeline_immediate[instruction_index];
            E_M_pipeline_instruction[instruction_index] = I_E_pipeline_instruction[instruction_index];

            //actual operation to be performed in sw_mem()
        }

        private void lw_mem(int instruction_index)
        {
            M_W_pipeline_RS[instruction_index] = E_M_pipeline_RS[instruction_index];
            M_W_pipeline_RT[instruction_index] = E_M_pipeline_RT[instruction_index];
            M_W_pipeline_immediate[instruction_index] = E_M_pipeline_immediate[instruction_index];
            M_W_pipeline_instruction[instruction_index] = E_M_pipeline_instruction[instruction_index];

            int mem_address = M_W_pipeline_RS[instruction_index] + M_W_pipeline_immediate[instruction_index];
            LW_SW_memory_address[instruction_index] = mem_address;

            MWpipe_RT_value[instruction_index] = memory_value[mem_address];
        }

        private void sw_mem(int instruction_index)
        {
            M_W_pipeline_RS[instruction_index] = E_M_pipeline_RS[instruction_index];
            M_W_pipeline_RT[instruction_index] = E_M_pipeline_RT[instruction_index];
            M_W_pipeline_immediate[instruction_index] = E_M_pipeline_immediate[instruction_index];
            M_W_pipeline_instruction[instruction_index] = E_M_pipeline_instruction[instruction_index];

            int mem_address = M_W_pipeline_RS[instruction_index] + M_W_pipeline_immediate[instruction_index];
            LW_SW_memory_address[instruction_index] = mem_address;
            MessageBox.Show("Address for LW/SW: "+LW_SW_memory_address[instruction_index].ToString());
            MessageBox.Show("RT register R: " + M_W_pipeline_RT[instruction_index]);
            MessageBox.Show("RS register R: " + M_W_pipeline_RS[instruction_index]);
            MessageBox.Show("Value in R8: " + registerSim.value[8].ToString());
            //MessageBox.Show(registerSim.value[M_W_pipeline_RT[instruction_index]].ToString());
            old_memory_values[instruction_index]=memory_value[LW_SW_memory_address[instruction_index]];
            memory_value[LW_SW_memory_address[instruction_index]] = registerSim.value[M_W_pipeline_RT[instruction_index]];
            //MessageBox.Show("New memory value: "+memory_value[LW_SW_memory_address[instruction_index]].ToString());
        }

        //used to reset everything back to original state.
        private void reset_btn_Click(object sender, EventArgs e)
        {
            memory_block_List.Items.Clear();
            Test_Status.Items.Clear();
            GPR_List.Items.Clear();
            GPR_Values.Items.Clear();
            instr_hold_box.Items.Clear();
            instr_counter.Items.Clear();
            prog_counter_box.Items.Clear();
            clock_cycle_box.Items.Clear();
            button1.Enabled = true;
            start_btn.Enabled = true;
            step_btn.Enabled = false;
            run_btn.Enabled = false;
            hexInstructions.Clear();
            binInstructions.Clear();
            instruction_count = 0;
            program_counter = 0;
            clock_cycle = 0;
            instruction_pipeline_1 = 0;
            instruction_pipeline_2 = 0;
            instruction_pipeline_3 = 0;
            instruction_pipeline_4 = 0;
            instruction_pipeline_5 = 0;
            instruction_pipeline_6 = 0;
            instruction_pipeline_chk_2 = 0;
            instruction_pipeline_chk_3 = 0;
            instruction_pipeline_chk_4 = 0;
            instruction_pipeline_chk_5 = 0;
            instruction_pipeline_chk_6 = 0;
            start_instruction_3 = 0;
            start_instruction_4 = 0;
            start_instruction_5 = 0;
            start_instruction_6 = 0;
            i = 0;
            j = 5;
        }

        //used to pull up help menu describing each buttons function
        private void help_btn_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Welcome to the Pipelined MIPS Procesor Simulator"+
                "\n*******************************************************"+
                "\nHere are basic instructions for using the program."+
                "\n\n"+
                "Add Instruction"+ 
                "\n--------------------"+
                "\nUser must input a 32 bit hexadecimal instruction."+
                "\nOnce button is pressed, if there is no problem with the instruction,"+
                "\nit is added to the instruction cache (List of Instructions box)."+
                "\nNOTE: ALL LETTERS IN INSTRUCTIONS MUST BE CAPITALIZED!"+
                "\nEX: ABCD1234"+
                "\nButton will be disabled once six instructions have been added"+
                "\nOR when Step button is pressed once." +
                "\n\n"+

                "Start"+
                "\n--------------------"+
                "\nWhen clicked, it populates 3 list boxes."+
                "\n1 list under 'Memory' simulating the block of memory used for memory to register and register to memory operations"+
                " (box under Address: Value)." +
                "\n2 lists under 'General Purpose Registers' (GPRs): 1 showing the appropiate names for each GPR"+
                " and 1 list showing the values in each GPR."+
                "\nButton will be disabled after being pressed once."+
                "\n\n"+

                "Step"+
                "\n--------------------"+
                "\nAllows the user to step through clock cycles one at a time to see stages of pipelining."+
                "\nButton will be disabled after final instruction completes last stage of instruction cycle."+
                "\n\n"+
                
                "Run"+
                "\n--------------------"+
                "\nAllows the user to run all clock cycles at once. Can be used at any time while using step button to execute remaining clock cycles."+
                "\n\n"+

                "Reset"+
                "\n--------------------"+
                "\nresets clock cycles, instruction count, program counter and returns all values in GPR"+
                " and memory to original values. Also clears Status box.");
        }

    }
}

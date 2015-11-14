namespace MIPS_ISA_GUI
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.instr_hold_box = new System.Windows.Forms.ListBox();
            this.instr_counter = new System.Windows.Forms.ListBox();
            this.Instr_Count_Label = new System.Windows.Forms.Label();
            this.List_Instr_Label = new System.Windows.Forms.Label();
            this.instr_input_textBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.start_btn = new System.Windows.Forms.Button();
            this.GPR_Values = new System.Windows.Forms.ListBox();
            this.GPR_List = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Test_Status = new System.Windows.Forms.ListBox();
            this.step_btn = new System.Windows.Forms.Button();
            this.prog_counter_box = new System.Windows.Forms.ListBox();
            this.prog_counter_label = new System.Windows.Forms.Label();
            this.clock_cycle_label = new System.Windows.Forms.Label();
            this.clock_cycle_box = new System.Windows.Forms.ListBox();
            this.memory_block_List = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.reset_btn = new System.Windows.Forms.Button();
            this.help_btn = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.run_btn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // instr_hold_box
            // 
            this.instr_hold_box.FormattingEnabled = true;
            this.instr_hold_box.Location = new System.Drawing.Point(169, 76);
            this.instr_hold_box.Name = "instr_hold_box";
            this.instr_hold_box.Size = new System.Drawing.Size(172, 43);
            this.instr_hold_box.TabIndex = 0;
            // 
            // instr_counter
            // 
            this.instr_counter.FormattingEnabled = true;
            this.instr_counter.Location = new System.Drawing.Point(364, 16);
            this.instr_counter.Name = "instr_counter";
            this.instr_counter.Size = new System.Drawing.Size(45, 17);
            this.instr_counter.TabIndex = 1;
            // 
            // Instr_Count_Label
            // 
            this.Instr_Count_Label.AutoSize = true;
            this.Instr_Count_Label.Location = new System.Drawing.Point(271, 19);
            this.Instr_Count_Label.Name = "Instr_Count_Label";
            this.Instr_Count_Label.Size = new System.Drawing.Size(87, 13);
            this.Instr_Count_Label.TabIndex = 2;
            this.Instr_Count_Label.Text = "Instruction Count";
            this.Instr_Count_Label.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // List_Instr_Label
            // 
            this.List_Instr_Label.AutoSize = true;
            this.List_Instr_Label.Location = new System.Drawing.Point(166, 50);
            this.List_Instr_Label.Name = "List_Instr_Label";
            this.List_Instr_Label.Size = new System.Drawing.Size(92, 13);
            this.List_Instr_Label.TabIndex = 3;
            this.List_Instr_Label.Text = "List of Instructions";
            // 
            // instr_input_textBox
            // 
            this.instr_input_textBox.Location = new System.Drawing.Point(115, 16);
            this.instr_input_textBox.Name = "instr_input_textBox";
            this.instr_input_textBox.Size = new System.Drawing.Size(133, 20);
            this.instr_input_textBox.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Instruction Input";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(42, 70);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(103, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "Add Instruction";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // start_btn
            // 
            this.start_btn.Location = new System.Drawing.Point(44, 109);
            this.start_btn.Name = "start_btn";
            this.start_btn.Size = new System.Drawing.Size(103, 23);
            this.start_btn.TabIndex = 7;
            this.start_btn.Text = "Start";
            this.start_btn.UseVisualStyleBackColor = true;
            this.start_btn.Click += new System.EventHandler(this.start_btn_Click);
            // 
            // GPR_Values
            // 
            this.GPR_Values.FormattingEnabled = true;
            this.GPR_Values.Location = new System.Drawing.Point(582, 60);
            this.GPR_Values.Name = "GPR_Values";
            this.GPR_Values.Size = new System.Drawing.Size(101, 134);
            this.GPR_Values.TabIndex = 8;
            // 
            // GPR_List
            // 
            this.GPR_List.FormattingEnabled = true;
            this.GPR_List.Location = new System.Drawing.Point(438, 60);
            this.GPR_List.Name = "GPR_List";
            this.GPR_List.Size = new System.Drawing.Size(101, 134);
            this.GPR_List.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(486, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(133, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "General Purpose Registers";
            // 
            // Test_Status
            // 
            this.Test_Status.FormattingEnabled = true;
            this.Test_Status.Location = new System.Drawing.Point(364, 236);
            this.Test_Status.Name = "Test_Status";
            this.Test_Status.Size = new System.Drawing.Size(318, 173);
            this.Test_Status.TabIndex = 11;
            // 
            // step_btn
            // 
            this.step_btn.Location = new System.Drawing.Point(239, 164);
            this.step_btn.Name = "step_btn";
            this.step_btn.Size = new System.Drawing.Size(103, 23);
            this.step_btn.TabIndex = 12;
            this.step_btn.Text = "Step";
            this.step_btn.UseVisualStyleBackColor = true;
            this.step_btn.Click += new System.EventHandler(this.decode_btn_Click);
            // 
            // prog_counter_box
            // 
            this.prog_counter_box.FormattingEnabled = true;
            this.prog_counter_box.Location = new System.Drawing.Point(115, 165);
            this.prog_counter_box.Name = "prog_counter_box";
            this.prog_counter_box.Size = new System.Drawing.Size(101, 17);
            this.prog_counter_box.TabIndex = 13;
            // 
            // prog_counter_label
            // 
            this.prog_counter_label.AutoSize = true;
            this.prog_counter_label.Location = new System.Drawing.Point(23, 169);
            this.prog_counter_label.Name = "prog_counter_label";
            this.prog_counter_label.Size = new System.Drawing.Size(86, 13);
            this.prog_counter_label.TabIndex = 14;
            this.prog_counter_label.Text = "Program Counter";
            // 
            // clock_cycle_label
            // 
            this.clock_cycle_label.AutoSize = true;
            this.clock_cycle_label.Location = new System.Drawing.Point(30, 199);
            this.clock_cycle_label.Name = "clock_cycle_label";
            this.clock_cycle_label.Size = new System.Drawing.Size(63, 13);
            this.clock_cycle_label.TabIndex = 15;
            this.clock_cycle_label.Text = "Clock Cycle";
            // 
            // clock_cycle_box
            // 
            this.clock_cycle_box.FormattingEnabled = true;
            this.clock_cycle_box.Location = new System.Drawing.Point(99, 199);
            this.clock_cycle_box.Name = "clock_cycle_box";
            this.clock_cycle_box.Size = new System.Drawing.Size(101, 17);
            this.clock_cycle_box.TabIndex = 16;
            // 
            // memory_block_List
            // 
            this.memory_block_List.FormattingEnabled = true;
            this.memory_block_List.Location = new System.Drawing.Point(25, 281);
            this.memory_block_List.Name = "memory_block_List";
            this.memory_block_List.Size = new System.Drawing.Size(191, 121);
            this.memory_block_List.TabIndex = 17;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(96, 236);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 13);
            this.label3.TabIndex = 18;
            this.label3.Text = "Memory";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(78, 265);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 13);
            this.label4.TabIndex = 19;
            this.label4.Text = "Address: Value";
            // 
            // reset_btn
            // 
            this.reset_btn.Location = new System.Drawing.Point(238, 281);
            this.reset_btn.Name = "reset_btn";
            this.reset_btn.Size = new System.Drawing.Size(103, 23);
            this.reset_btn.TabIndex = 20;
            this.reset_btn.Text = "Reset";
            this.reset_btn.UseVisualStyleBackColor = true;
            this.reset_btn.Click += new System.EventHandler(this.reset_btn_Click);
            // 
            // help_btn
            // 
            this.help_btn.Location = new System.Drawing.Point(238, 335);
            this.help_btn.Name = "help_btn";
            this.help_btn.Size = new System.Drawing.Size(103, 23);
            this.help_btn.TabIndex = 21;
            this.help_btn.Text = "Help";
            this.help_btn.UseVisualStyleBackColor = true;
            this.help_btn.Click += new System.EventHandler(this.help_btn_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(502, 220);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(37, 13);
            this.label5.TabIndex = 22;
            this.label5.Text = "Status";
            // 
            // run_btn
            // 
            this.run_btn.Location = new System.Drawing.Point(239, 210);
            this.run_btn.Name = "run_btn";
            this.run_btn.Size = new System.Drawing.Size(102, 23);
            this.run_btn.TabIndex = 23;
            this.run_btn.Text = "Run";
            this.run_btn.UseVisualStyleBackColor = true;
            this.run_btn.Click += new System.EventHandler(this.run_btn_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(707, 438);
            this.Controls.Add(this.run_btn);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.help_btn);
            this.Controls.Add(this.reset_btn);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.memory_block_List);
            this.Controls.Add(this.clock_cycle_box);
            this.Controls.Add(this.clock_cycle_label);
            this.Controls.Add(this.prog_counter_label);
            this.Controls.Add(this.prog_counter_box);
            this.Controls.Add(this.step_btn);
            this.Controls.Add(this.Test_Status);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.GPR_List);
            this.Controls.Add(this.GPR_Values);
            this.Controls.Add(this.start_btn);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.instr_input_textBox);
            this.Controls.Add(this.List_Instr_Label);
            this.Controls.Add(this.Instr_Count_Label);
            this.Controls.Add(this.instr_counter);
            this.Controls.Add(this.instr_hold_box);
            this.Name = "Form1";
            this.Text = "Pipelined MIPS Processor Simulator";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox instr_hold_box;
        private System.Windows.Forms.ListBox instr_counter;
        private System.Windows.Forms.Label Instr_Count_Label;
        private System.Windows.Forms.Label List_Instr_Label;
        private System.Windows.Forms.TextBox instr_input_textBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button start_btn;
        private System.Windows.Forms.ListBox GPR_Values;
        private System.Windows.Forms.ListBox GPR_List;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox Test_Status;
        private System.Windows.Forms.Button step_btn;
        private System.Windows.Forms.ListBox prog_counter_box;
        private System.Windows.Forms.Label prog_counter_label;
        private System.Windows.Forms.Label clock_cycle_label;
        private System.Windows.Forms.ListBox clock_cycle_box;
        private System.Windows.Forms.ListBox memory_block_List;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button reset_btn;
        private System.Windows.Forms.Button help_btn;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button run_btn;
    }
}


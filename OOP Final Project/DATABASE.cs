using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

public class PersonalFinanceApp : Form
{
    private SqlConnection connection;
    private ListBox transactionsListBox;
    private TextBox descriptionTextBox;
    private TextBox amountTextBox;
    private Button addIncomeButton;
    private Button addExpenseButton;
    private Button openDatabaseButton;
    private Button totalIncomeButton;
    private Button totalExpenseButton;
    private Label descriptionLabel;
    private Label amountLabel;

    public PersonalFinanceApp()
    {
        InitializeComponents();


        string connectionString = @"Data Source=4567321N-\SQLEXPRESS; Initial Catalog=Personal Finance;Integrated Security=True";
        connection = new SqlConnection(connectionString);
    }

    private void InitializeComponents()
    {

        transactionsListBox = new ListBox();
        descriptionTextBox = new TextBox();
        amountTextBox = new TextBox();
        addIncomeButton = new Button();
        addExpenseButton = new Button();
        openDatabaseButton = new Button();
        totalIncomeButton = new Button();
        totalExpenseButton = new Button();
        descriptionLabel = new Label();
        amountLabel = new Label();


        transactionsListBox.Location = new Point(700, 12);
        transactionsListBox.Size = new Size(400, 300);

        descriptionTextBox.Location = new Point(700, 320);
        descriptionTextBox.Size = new Size(400, 20);

        amountTextBox.Location = new Point(700, 350);
        amountTextBox.Size = new Size(150, 20);

        addIncomeButton.Location = new Point(500, 400);
        addIncomeButton.Size = new Size(150, 35);
        addIncomeButton.Text = "Add Income";
        addIncomeButton.Click += AddIncomeButton_Click;

        addExpenseButton.Location = new Point(680, 400);
        addExpenseButton.Size = new Size(150, 35);
        addExpenseButton.Text = "Add Expense";
        addExpenseButton.Click += AddExpenseButton_Click;

        openDatabaseButton.Location = new Point(860, 400);
        openDatabaseButton.Size = new Size(150, 35);
        openDatabaseButton.Text = "Open List";
        openDatabaseButton.Click += OpenDatabaseButton_Click;

        totalIncomeButton.Location = new Point(1040, 400);
        totalIncomeButton.Size = new Size(150, 35);
        totalIncomeButton.Text = "Total Income";
        totalIncomeButton.Click += TotalIncomeButton_Click;

        totalExpenseButton.Location = new Point(1220, 400);
        totalExpenseButton.Size = new Size(150, 35);
        totalExpenseButton.Text = "Total Expense";
        totalExpenseButton.Click += TotalExpenseButton_Click;

        descriptionLabel.Location = new Point(580, 325);
        descriptionLabel.Text = "Description:";

        amountLabel.Location = new Point(535, 355);
        amountLabel.Size = new Size(200, 30);
        amountLabel.Text = "Amount of Money:";


        Controls.Add(transactionsListBox);
        Controls.Add(descriptionTextBox);
        Controls.Add(amountTextBox);
        Controls.Add(addIncomeButton);
        Controls.Add(addExpenseButton);
        Controls.Add(openDatabaseButton);
        Controls.Add(totalIncomeButton);
        Controls.Add(totalExpenseButton);
        Controls.Add(descriptionLabel);
        Controls.Add(amountLabel);


        Size = new Size(500, 380);
        Text = "Personal Finance Manager";
    }

    private void AddIncomeButton_Click(object sender, EventArgs e)
    {
        string description = descriptionTextBox.Text;
        decimal amount = decimal.Parse(amountTextBox.Text);

        InsertTransaction(description, amount, "Income");
        LoadTransactions();
    }

    private void AddExpenseButton_Click(object sender, EventArgs e)
    {
        string description = descriptionTextBox.Text;
        decimal amount = decimal.Parse(amountTextBox.Text);

        InsertTransaction(description, -amount, "Expense");
        LoadTransactions();
    }

    private void OpenDatabaseButton_Click(object sender, EventArgs e)
    {
        LoadTransactions();
    }

    private void TotalIncomeButton_Click(object sender, EventArgs e)
    {
        CalculateTotal("Income");
    }

    private void TotalExpenseButton_Click(object sender, EventArgs e)
    {
        CalculateTotal("Expense");
    }

    private void InsertTransaction(string description, decimal amount, string transactionType)
    {
        try
        {
            connection.Open();

            string insertQuery = "INSERT INTO Transactions (Description, Amount, TransactionType) " +
                "VALUES (@Description, @Amount, @TransactionType)";

            using (SqlCommand command = new SqlCommand(insertQuery, connection))
            {
                command.Parameters.AddWithValue("@Description", description);
                command.Parameters.AddWithValue("@Amount", amount);
                command.Parameters.AddWithValue("@TransactionType", transactionType);

                command.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error inserting transaction: " + ex.Message);
        }
        finally
        {
            connection.Close();
        }
    }

    private void LoadTransactions()
    {
        transactionsListBox.Items.Clear();

        try
        {
            connection.Open();

            string selectQuery = "SELECT Description, Amount, TransactionType FROM Transactions";

            using (SqlCommand command = new SqlCommand(selectQuery, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string description = reader.GetString(0);
                        decimal amount = reader.GetDecimal(1);
                        string transactionType = reader.GetString(2);

                        string transactionText = $"{description} | {amount.ToString("C2")} | {transactionType}";
                        transactionsListBox.Items.Add(transactionText);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error loading transactions: " + ex.Message);
        }
        finally
        {
            connection.Close();
        }
    }

    private void InitializeComponent()
    {

    }

    private void CalculateTotal(string transactionType)
    {
        decimal total = 0;

        try
        {
            connection.Open();

            string selectQuery = "SELECT Amount FROM Transactions WHERE TransactionType = @TransactionType";

            using (SqlCommand command = new SqlCommand(selectQuery, connection))
            {
                command.Parameters.AddWithValue("@TransactionType", transactionType);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        decimal amount = reader.GetDecimal(0);
                        total += amount;
                    }
                }
            }

            string totalText = $"{transactionType}: {total.ToString("C2")}";
            MessageBox.Show(totalText, "Total");
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error calculating total: " + ex.Message);
        }
        finally
        {
            connection.Close();
        }
    }

}
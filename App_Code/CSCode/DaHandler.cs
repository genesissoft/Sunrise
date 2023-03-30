namespace aaproject
{
	using System;
	using System.Collections;
	using System.Data;
	using System.Data.SqlClient;

	public class DaHandler
	{
		// Methods
		public DaHandler()
		{
		} 
       
		private static SqlCommand CreateSelectViaSPCommand(string spName, string strConn)
		{
			IEnumerator enumerator1=null;
			SqlConnection connection1 = new SqlConnection(strConn);
			SqlCommand command1 = new SqlCommand(spName, connection1);
			command1.CommandType = CommandType.StoredProcedure;
			SqlCommandBuilder builder1 = new SqlCommandBuilder();
			connection1.Open();				 
			SqlCommandBuilder.DeriveParameters(command1);		
			connection1.Close();
			try
			{
				enumerator1 = command1.Parameters.GetEnumerator();
				while (enumerator1.MoveNext())
				{
					SqlParameter parameter1 = (SqlParameter) enumerator1.Current;
					parameter1.SourceVersion = DataRowVersion.Current;
					parameter1.SourceColumn = parameter1.ParameterName.Remove(0, 1);
				}
			}
			finally
			{
				if (enumerator1 is IDisposable)
				{
					((IDisposable) enumerator1).Dispose();
				}
			}
			return command1;
		}

		public static bool SubmitChanges(ref DataSet DS, string ConnectionString, string spSelectName,string spUpDateName, string spInsertName, string spDeleteName)
		{
             
			DataTable table1 = DS.Tables[0];
			DaHandler.SubmitChanges(ref table1, ConnectionString, spSelectName, spUpDateName, spInsertName, spDeleteName);
			return true;
		}

		public static bool SubmitChanges(ref DataTable tbl, string ConnectionString, string spSelectName,string spUpDateName, string spInsertName, string spDeleteName)
		{
			SqlTransaction  trans =null;
			try
			{      
				SqlDataAdapter adapter1 = new SqlDataAdapter();			
				if (spSelectName!="")
				{
					adapter1.SelectCommand = DaHandler.CreateSelectViaSPCommand(spSelectName, ConnectionString);
				}
				SqlCommandBuilder bldr1 = new SqlCommandBuilder(adapter1);
				adapter1.InsertCommand =bldr1.GetInsertCommand();	
				// we don't requied forupdate and delete .
				//adapter1.UpdateCommand=bldr1.GetUpdateCommand();
				//adapter1.DeleteCommand =bldr1.GetDeleteCommand();
				adapter1.ContinueUpdateOnError = true;
				adapter1.InsertCommand.Connection.Open();					
				trans =  adapter1.InsertCommand.Connection.BeginTransaction();
				adapter1.InsertCommand.Transaction =trans;
				//adapter1.UpdateCommand.Transaction =trans;
				//adapter1.DeleteCommand.Transaction =trans;
				adapter1.Update(tbl);
				trans.Commit();
				if(adapter1.InsertCommand.Connection.State ==ConnectionState.Open)
					adapter1.InsertCommand.Connection.Close();
			}
			catch (Exception ex)
			{
				trans.Rollback();
				System.Diagnostics.Debug.WriteLine (ex.Message +ex.StackTrace);                
				bool flag1 = false;                
				return flag1;                
			}
			return true;
		}
	}
}


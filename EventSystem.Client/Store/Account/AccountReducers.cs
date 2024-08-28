using Fluxor;

namespace EventSystem.Client.Store.Account
{
    public static class AccountReducers
    {
        // Login Reducers
        [ReducerMethod]
        public static AccountState ReduceLoginAction(AccountState state, LoginAction action) =>
            new AccountState(state.UserId, state.FullName, state.Token, false, true, false, null, null, 0, false);

        [ReducerMethod]
        public static AccountState ReduceLoginSuccessAction(AccountState state, LoginSuccessAction action) =>
            new AccountState(action.LoginResponseModel.UserId, 
                action.LoginResponseModel.FullName, 
                action.LoginResponseModel.Token, 
                true, false, true,
                action.LoginResponseModel.Role,
                null, 
                action.LoginResponseModel.ExpiresIn,
                true);

        [ReducerMethod]
        public static AccountState ReduceLoginFailedAction(AccountState state, LoginFailedAction action) =>
            new AccountState(state.UserId, state.FullName, state.Token, false, false, false, null, action.ErrorMessage, 0, true);

        // Register Reducers
        [ReducerMethod]
        public static AccountState ReduceRegisterAction(AccountState state, RegisterAction action) =>
            new AccountState(state.UserId, state.FullName, state.Token, false, true, false, null, null, 0, false);

        [ReducerMethod]
        public static AccountState ReduceRegisterSuccessAction(AccountState state, RegisterSuccessAction action) =>
            new AccountState("", "", "", false, false, true, null, action.Message, 0, true);

        [ReducerMethod]
        public static AccountState ReduceRegisterFailedAction(AccountState state, RegisterFailedAction action) =>
            new AccountState(state.UserId, state.FullName, state.Token, false, false, false, null, action.Message, 0, true);

        // Logout Reducer
        [ReducerMethod]
        public static AccountState ReduceLogoutAction(AccountState state, LogoutAction action) =>
            new AccountState(null, null, null, false, false, true, null, null, 0, true);
    }

}

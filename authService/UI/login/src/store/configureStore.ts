import { createStore, combineReducers } from 'redux';
import authenticationReducer from '../reducers/authentication';

export default () => {
    const store = createStore(
        combineReducers({
            authentication: authenticationReducer
        })
    );
    
    return store;
};

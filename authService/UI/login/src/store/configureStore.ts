import {createStore, combineReducers, applyMiddleware} from 'redux';
import authenticationReducer from '../reducers/authentication';
import thunk from "redux-thunk";

export default () => {
    const store = createStore(
        combineReducers({
            authentication: authenticationReducer
        }),
        applyMiddleware(thunk)
    );
    
    return store;
};

// @flow
import { combineReducers } from 'redux'
import { connectRouter } from 'connected-react-router'
import docinsReducer from './docinsReducer'

export default function createRootReducer (history: History) {
  return combineReducers({
    router: connectRouter(history),
    docins: docinsReducer
  })
}

import {
  SET_CHARACTERS_DOCIN_VISIBILITY,
  SET_OUTLINE_DOCIN_VISIBILITY
} from '../actions/docinsModule'

const initialState = {
  isDocinsContainerVisible: false,
  charactersDocin: {
    isVisible: false
  },
  outlineDocin: {
    isVisible: false
  }
}

const isAnyDocinVisible = state => {
  return state.charactersDocin.isVisible || state.outlineDocin.isVisible
}

export default function reducer (state = initialState, action) {
  switch (action.type) {
    case SET_CHARACTERS_DOCIN_VISIBILITY:
      const newState = {
        ...state,
        charactersDocin: {
          ...state.charactersDocin,
          isVisible: action.data
        }
      }
      newState.isDocinsContainerVisible = isAnyDocinVisible(newState)
      return newState

    case SET_OUTLINE_DOCIN_VISIBILITY:
      const newState_UGH = {
        ...state,
        outlineDocin: {
          ...state.outlineDocin,
          isVisible: action.data
        }
      }
      newState_UGH.isDocinsContainerVisible = isAnyDocinVisible(newState_UGH)
      return newState_UGH

    default:
      return state
  }
}

export const SET_CHARACTERS_DOCIN_VISIBILITY =
  'docrider/docins/SET_CHARACTERS_DOCIN_VISIBILITY'
export const SET_OUTLINE_DOCIN_VISIBILITY =
  'docrider/docins/SET_OUTLINE_DOCIN_VISIBILITY'

export const setCharactersDocinVisibility = isVisible => ({
  type: SET_CHARACTERS_DOCIN_VISIBILITY,
  data: isVisible
})

export const setOutlineDocinVisibility = isVisible => ({
  type: SET_OUTLINE_DOCIN_VISIBILITY,
  data: isVisible
})

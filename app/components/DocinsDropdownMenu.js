import React from 'react'
import { connect } from 'react-redux'

import docinsMenuStyles from '../styles/docins/docinsMenu.scss'
import * as docinsModule from '../actions/docinsModule'

class DocinsDropdownMenu extends React.Component {
  constructor (props) {
    super(props)
    this.state = {
      isOutlineDocinToggled: false,
      isScenesDocinToggled: false,
      isCharactersDocinToggled: false,
      isInteractionsDocinToggled: false,
      isDialoguesDocinToggled: false,
      isThoughtBubblesDocinToggled: false
    }
  }

  renderToggle (isToggled) {
    return isToggled ? (
      <i className='fas fa-toggle-on' />
    ) : (
      <i className='fas fa-toggle-off' />
    )
  }

  render () {
    return (
      <div className={docinsMenuStyles.docinsMenu}>
        <div className={docinsMenuStyles.dropdownButton}>
          Docins
          <span className={docinsMenuStyles.dropdownIcon}>
            <i className='fas fa-caret-square-down' />
          </span>
        </div>

        <div className={docinsMenuStyles.dropdownMenu}>
          <ul>
            {this.renderOutlineDocinToggle()}
            {this.renderToggleable('Scenes', 'isScenesDocinToggled')}
            {this.renderCharactersDocinToggle()}
            {this.renderToggleable(
              'Interactions',
              'isInteractionsDocinToggled'
            )}
            {this.renderToggleable('Dialogues', 'isDialoguesDocinToggled')}
            {this.renderToggleable(
              'Thought bubbles',
              'isThoughtBubblesDocinToggled'
            )}
          </ul>
        </div>
      </div>
    )
  }

  renderCharactersDocinToggle () {
    const {
      isCharactersDocinVisible,
      setCharactersDocinVisibility
    } = this.props

    let toggleIcon
    if (isCharactersDocinVisible) {
      toggleIcon = <i className='fas fa-toggle-on' />
    } else {
      toggleIcon = (
        <i
          className='fas fa-toggle-off'
          style={{ color: 'rgba(0, 0, 0, 0.5)' }}
        />
      )
    }

    return (
      <li
        onClick={() => setCharactersDocinVisibility(!isCharactersDocinVisible)}
      >
        Characters
        {toggleIcon}
      </li>
    )
  }

  renderOutlineDocinToggle () {
    const { isOutlineDocinVisible, setOutlineDocinVisibility } = this.props

    let toggleIcon
    if (isOutlineDocinVisible) {
      toggleIcon = <i className='fas fa-toggle-on' />
    } else {
      toggleIcon = (
        <i
          className='fas fa-toggle-off'
          style={{ color: 'rgba(0, 0, 0, 0.5)' }}
        />
      )
    }

    return (
      <li onClick={() => setOutlineDocinVisibility(!isOutlineDocinVisible)}>
        Outline
        {toggleIcon}
      </li>
    )
  }

  renderToggleable (text, toggleStateName) {
    const isToggled = this.state[toggleStateName]

    const handleClick = () => {
      this.setState({
        ...this.state,
        [toggleStateName]: !isToggled
      })
    }

    let toggleIcon

    if (isToggled) {
      toggleIcon = <i className='fas fa-toggle-on' />
    } else {
      toggleIcon = (
        <i
          className='fas fa-toggle-off'
          style={{ color: 'rgba(0, 0, 0, 0.5)' }}
        />
      )
    }

    return (
      <li onClick={handleClick}>
        {text}
        {toggleIcon}
      </li>
    )
  }
}

export default connect(
  state => ({
    isCharactersDocinVisible: state.docins.charactersDocin.isVisible,
    isOutlineDocinVisible: state.docins.outlineDocin.isVisible
  }),
  dispatch => ({
    setCharactersDocinVisibility: isVisible =>
      dispatch(docinsModule.setCharactersDocinVisibility(isVisible)),
    setOutlineDocinVisibility: isVisible =>
      dispatch(docinsModule.setOutlineDocinVisibility(isVisible))
  })
)(DocinsDropdownMenu)

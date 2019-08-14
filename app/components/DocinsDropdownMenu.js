import React from 'react'

import docinsMenuStyles from '../styles/docinsMenu.scss'

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
            {this.renderToggleable('Outline', 'isOutlineDocinToggled')}
            {this.renderToggleable('Scenes', 'isScenesDocinToggled')}
            {this.renderToggleable('Characters', 'isCharactersDocinToggled')}
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

export default DocinsDropdownMenu

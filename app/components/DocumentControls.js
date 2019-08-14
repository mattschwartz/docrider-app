import React from 'react'

import docriderStyles from '../styles/docrider.scss'
import DocinsDropdownMenu from './DocinsDropdownMenu'

class DocumentControls extends React.Component {
  constructor (props) {
    super(props)
    this.state = {
      docFile: {
        filepath: './documents/rockin-files/docrider.dr'
      },
      docFileStatus: 'changes pending'
    }
  }

  render () {
    return (
      <div className={docriderStyles.controlsContainer}>
        <div className={docriderStyles.btnGroup}>
          <button type='button'>
            <i className='fas fa-file-alt' />
          </button>
          <button type='button' onClick={this.props.onSaveClicked}>
            <i className='fas fa-save' />
          </button>
          <button type='button'>
            <i className='fas fa-folder-open' />
          </button>
        </div>
        <DocinsDropdownMenu />
      </div>
    )
  }
}

export default DocumentControls

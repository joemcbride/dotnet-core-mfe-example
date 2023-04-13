import { gql, useMutation } from '@apollo/client'
import { useFormik } from 'formik'

const CREATE_JOB_MUTATION = gql`
  mutation CreateJob($dataFileOnly: Boolean!) {
    createJob(input: { dataFileOnly: $dataFileOnly }) {
      jobId
      status
      startedAtTimeUtc
    }
  }
`

export default function CreateJob() {
  const [createJob, { data, loading, error }] = useMutation(CREATE_JOB_MUTATION)

  const formik = useFormik({
    initialValues: {
      dataFileOnly: true,
    },
    onSubmit: async (values) => {
      console.log('form values', values)
      try {
        await createJob({
          variables: {
            dataFileOnly: values.dataFileOnly,
          },
        })
      } catch (e) {
        console.log(e)
      }
    },
  })

  if (loading) return 'Submitting...'
  if (error) return `Submission error! ${error.message}`

  if (data) {
    console.log('submit result', data)
  }

  return (
    <form onSubmit={formik.handleSubmit}>
      <div>
        <input
          id="dataFileOnly"
          name="dataFileOnly"
          type="checkbox"
          onChange={formik.handleChange}
          checked={formik.values.dataFileOnly}
        />
        <label htmlFor="dataFileOnly">Create Data File Only</label>
      </div>
      <button type="submit">Submit</button>
    </form>
  )
}
